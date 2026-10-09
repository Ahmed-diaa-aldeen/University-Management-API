using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api1.DTOS;
using api1.Models;
using api1.Repository.Generic;

namespace api1.Repository.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Student> Students { get; }
        IGenericRepository<Course> Courses { get; }
        IGenericRepository<Enrollment> Enrollments { get; }

        // 1. جلب بروفايل الطالب كاملاً بالاسم
        Task<StudentDetailsDto?> GetStudentDetailsByNameAsync(string name);

        // 2. تسجيل طالب في كورس ورصد الدرجة وإعادة حساب الـ GPA
        Task<bool> EnrollStudentInCourseAsync(int studentId, int courseId, double grade);

        // 3. إلغاء تسجيل طالب من مادة وإعادة حساب الـ GPA
        Task<bool> UnenrollStudentFromCourseAsync(int studentId, int courseId);

        // 4. جلب أوائل الطلاب مرتبين حسب الـ GPA
        Task<IEnumerable<Student>> GetTopStudentsAsync(int count);

        // 5. جلب إحصائيات مادة معينة (المتوسط، الأعلى، والأدنى)
        Task<CourseStatsDto?> GetCourseStatisticsAsync(int courseId);

        Task<int> CompleteAsync();
    }
}