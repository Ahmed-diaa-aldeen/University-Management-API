using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using api1.DTOS;
using api1.Models;
using api1.Repository.Generic;

namespace api1.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MVCContext _context;

        public IGenericRepository<Student> Students { get; private set; }
        public IGenericRepository<Course> Courses { get; private set; }
        public IGenericRepository<Enrollment> Enrollments { get; private set; }

        public UnitOfWork(MVCContext context)
        {
            _context = context;
            Students = new GenericRepository<Student>(_context);
            Courses = new GenericRepository<Course>(_context);
            Enrollments = new GenericRepository<Enrollment>(_context);
        }

        // جلب تفاصيل الطالب ومواده والـ GPA باستخدام اسمه
        public async Task<StudentDetailsDto?> GetStudentDetailsByNameAsync(string name)
        {
            var student = await _context.Students
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                .FirstOrDefaultAsync(s => s.Name.ToLower() == name.ToLower());

            if (student == null) return null;

            return new StudentDetailsDto
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                GPA = student.GPA,
                Courses = student.Enrollments.Select(e => new StudentCourseDto
                {
                    CourseTitle = e.Course != null ? e.Course.Title : "غير محدد",
                    CreditHours = e.Course != null ? e.Course.CreditHours : 0,
                    Grade = e.Grade // التقدير الحرفي مثل A, B, C
                }).ToList()
            };
        }

        // تسجيل طالب في مادة (بـ 3 معاملات متوافقة تماماً مع الـ IUnitOfWork)
        public async Task<bool> EnrollStudentInCourseAsync(int studentId, int courseId, double grade)
        {
            var student = await _context.Students.FindAsync(studentId);
            var course = await _context.Courses.FindAsync(courseId);

            if (student == null || course == null) return false;

            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseId = courseId,
                MidtermScore = grade, // تخزين الدرجة كدرجة أساسية
                FinalScore = 0,
                Grade = CalculateGradeLetter(grade) // تحويل الدرجة لتقدير حرفي تلقائياً
            };

            await Enrollments.AddAsync(enrollment);
            await _context.SaveChangesAsync();

            student.GPA = await CalculateGPAAsync(studentId);
            _context.Students.Update(student);
            await _context.SaveChangesAsync();

            return true;
        }

        // إلغاء تسجيل طالب من مادة وإعادة حساب الـ GPA
        public async Task<bool> UnenrollStudentFromCourseAsync(int studentId, int courseId)
        {
            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);

            if (enrollment == null) return false;

            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();

            // إعادة حساب الـ GPA فوراً بعد حذف المادة
            var student = await _context.Students.FindAsync(studentId);
            if (student != null)
            {
                student.GPA = await CalculateGPAAsync(studentId);
                _context.Students.Update(student);
                await _context.SaveChangesAsync();
            }

            return true;
        }

        // جلب أوائل الطلاب (المرتبين أعلى GPA)
        public async Task<IEnumerable<Student>> GetTopStudentsAsync(int count)
        {
            return await _context.Students
                .OrderByDescending(s => s.GPA)
                .Take(count)
                .ToListAsync();
        }

        // حساب إحصائيات المادة (عدد المسجلين، المتوسط، الأعلى، والأقل)
        public async Task<CourseStatsDto?> GetCourseStatisticsAsync(int courseId)
        {
            var course = await _context.Courses
                .Include(c => c.Enrollments)
                .FirstOrDefaultAsync(c => c.Id == courseId);

            if (course == null) return null;

            var scores = course.Enrollments.Select(e => e.TotalScore).ToList();

            return new CourseStatsDto
            {
                CourseId = course.Id,
                CourseTitle = course.Title,
                TotalEnrolledStudents = scores.Count,
                AverageGrade = scores.Any() ? Math.Round(scores.Average(), 2) : 0,
                MaxGrade = scores.Any() ? scores.Max() : 0,
                MinGrade = scores.Any() ? scores.Min() : 0
            };
        }

        // دالة مساعدة لحساب الـ GPA
        private async Task<double> CalculateGPAAsync(int studentId)
        {
            var enrollments = await _context.Enrollments
                .Include(e => e.Course)
                .Where(e => e.StudentId == studentId)
                .ToListAsync();

            if (!enrollments.Any()) return 0.0;

            double totalPoints = 0;
            int totalHours = 0;

            foreach (var item in enrollments)
            {
                double points = ConvertGradeTo4PointScale(item.TotalScore);
                totalPoints += points * item.Course!.CreditHours;
                totalHours += item.Course.CreditHours;
            }

            return totalHours == 0 ? 0.0 : Math.Round(totalPoints / totalHours, 2);
        }

        private double ConvertGradeTo4PointScale(double percentage)
        {
            if (percentage >= 90) return 4.0;
            if (percentage >= 85) return 3.7;
            if (percentage >= 80) return 3.3;
            if (percentage >= 75) return 3.0;
            if (percentage >= 70) return 2.7;
            if (percentage >= 65) return 2.4;
            if (percentage >= 60) return 2.0;
            return 0.0;
        }

        private string CalculateGradeLetter(double percentage)
        {
            if (percentage >= 90) return "A+";
            if (percentage >= 85) return "A";
            if (percentage >= 80) return "B+";
            if (percentage >= 75) return "B";
            if (percentage >= 70) return "C+";
            if (percentage >= 65) return "C";
            if (percentage >= 60) return "D";
            return "F";
        }

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
        public void Dispose() => _context.Dispose();
    }
}