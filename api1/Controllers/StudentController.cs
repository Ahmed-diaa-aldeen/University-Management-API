
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using api1.Models;
using api1.DTOS;
using api1.Repository.UnitOfWork;

namespace api1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // قفل الكنترولر بالكامل
    public class StudentController(IUnitOfWork unitOfWork) : ControllerBase
    {
        // =========================================================
        // 1. عرض جميع الطلاب مع الـ GPA الحالي
        // =========================================================

        [HttpGet]
        [Authorize(Roles = "Admin,Student")]
        public async Task<IActionResult> GetAll()
        {
            var students = await unitOfWork.Students.GetAllAsync();

            return Ok(students);
        }


        // =========================================================
        // 2. البحث عن طالب بالاسم وعرض بياناته والمواد والـ GPA
        // =========================================================

        [HttpGet("details/{name}")]
        [Authorize(Roles = "Admin,Student")]
        public async Task<IActionResult> GetByName(string name)
        {
            var studentDetails =
                await unitOfWork.GetStudentDetailsByNameAsync(name);

            if (studentDetails == null)
            {
                return NotFound(
                    $"عفواً، لم يتم العثور على طالب باسم: {name}"
                );
            }

            return Ok(studentDetails);
        }


        // =========================================================
        // 3. عرض أوائل الطلاب
        // =========================================================

        [HttpGet("top/{count}")]
        [Authorize(Roles = "Admin,Student")]
        public async Task<IActionResult> GetTopStudents(int count = 5)
        {
            var topStudents =
                await unitOfWork.GetTopStudentsAsync(count);

            return Ok(topStudents);
        }


        // =========================================================
        // 4. عرض إحصائيات المادة
        // =========================================================

        [HttpGet("course-stats/{courseId}")]
        [Authorize(Roles = "Admin,Student")]
        public async Task<IActionResult> GetCourseStats(int courseId)
        {
            var stats =
                await unitOfWork.GetCourseStatisticsAsync(courseId);

            if (stats == null)
            {
                return NotFound(
                    "لم يتم العثور على المادة المحددة."
                );
            }

            return Ok(stats);
        }


        // =========================================================
        // 5. إضافة طالب جديد - Admin فقط
        // =========================================================

        [HttpPost("student")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddStudent(
            [FromBody] Student student)
        {
            await unitOfWork.Students.AddAsync(student);

            await unitOfWork.CompleteAsync();

            return Ok(student);
        }


        // =========================================================
        // 6. إضافة كورس دراسي جديد - Admin فقط
        // =========================================================

        [HttpPost("course")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCourse(
            [FromBody] Course course)
        {
            await unitOfWork.Courses.AddAsync(course);

            await unitOfWork.CompleteAsync();

            return Ok(course);
        }


        // =========================================================
        // 7. تسجيل كورس ورصد درجة الطالب وتحديث GPA
        // =========================================================

        [HttpPost("enroll")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EnrollStudent(
            int studentId,
            int courseId,
            double grade)
        {
            var success =
                await unitOfWork.EnrollStudentInCourseAsync(
                    studentId,
                    courseId,
                    grade
                );

            if (!success)
            {
                return BadRequest(
                    "عفواً، لم يتم العثور على الطالب أو الكورس المحدد."
                );
            }

            await unitOfWork.CompleteAsync();

            return Ok(
                "تم تسجيل الطالب في المادة وتحديث الـ GPA الخاص به بنجاح."
            );
        }


        // =========================================================
        // 8. إلغاء تسجيل مادة لطالب وإعادة حساب GPA
        // =========================================================

        [HttpDelete("unenroll")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UnenrollStudent(
            int studentId,
            int courseId)
        {
            var success =
                await unitOfWork.UnenrollStudentFromCourseAsync(
                    studentId,
                    courseId
                );

            if (!success)
            {
                return NotFound(
                    "عفواً، لم يتم العثور على تسجيل لهذه المادة لدى الطالب المحدد."
                );
            }

            await unitOfWork.CompleteAsync();

            return Ok(
                "تم إلغاء تسجيل المادة وتحديث الـ GPA بنجاح."
            );
        }


        // =========================================================
        // 9. رفع صورة بروفايل حقيقية
        // Student / Admin
        // =========================================================

        /// <summary>
        /// رفع صورة بروفايل حقيقية وتخزينها على السيرفر
        /// داخل مجلد wwwroot/uploads
        /// </summary>

        [HttpPost("upload-picture")]
        [Authorize(Roles = "Admin,Student")]
        public async Task<IActionResult> UploadProfilePicture(
            IFormFile file,
            [FromServices] IWebHostEnvironment env)
        {
            // ---------------------------------------------------------
            // أ) التحقق من وجود الملف
            // ---------------------------------------------------------

            if (file == null || file.Length == 0)
            {
                return BadRequest(
                    "يرجى اختيار صورة صالحة للرفع."
                );
            }


            // ---------------------------------------------------------
            // ب) التحقق من امتداد الملف
            // ---------------------------------------------------------

            var allowedExtensions = new[]
            {
                ".jpg",
                ".jpeg",
                ".png",
                ".webp"
            };

            var extension =
                Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
            {
                return BadRequest(
                    "صيغة الملف غير مدعومة. يرجى رفع صورة بصيغة JPG أو PNG أو WEBP."
                );
            }


            // ---------------------------------------------------------
            // ج) التحقق من حجم الملف
            // الحد الأقصى 2 ميجابايت
            // ---------------------------------------------------------

            if (file.Length > 2 * 1024 * 1024)
            {
                return BadRequest(
                    "حجم الصورة كبير جداً. الحد الأقصى المسموح به هو 2 ميجابايت."
                );
            }


            // ---------------------------------------------------------
            // د) استخراج اسم المستخدم من الـ Claims
            // ---------------------------------------------------------

            var username =
                User.FindFirstValue(ClaimTypes.Name);

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized(
                    "التوكن غير صالح."
                );
            }


            // ---------------------------------------------------------
            // هـ) إنشاء مسار التخزين داخل wwwroot/uploads
            // ---------------------------------------------------------

            var uploadsFolder =
                Path.Combine(
                    env.WebRootPath ??
                    Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot"
                    ),
                    "uploads"
                );


            // ---------------------------------------------------------
            // و) إنشاء المجلد إذا لم يكن موجوداً
            // ---------------------------------------------------------

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }


            // ---------------------------------------------------------
            // ز) توليد اسم فريد للملف
            // ---------------------------------------------------------

            var uniqueFileName =
                $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";

            var filePath =
                Path.Combine(
                    uploadsFolder,
                    uniqueFileName
                );


            // ---------------------------------------------------------
            // ح) حفظ الملف على السيرفر
            // ---------------------------------------------------------

            using (var stream =
                   new FileStream(
                       filePath,
                       FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }


            // ---------------------------------------------------------
            // ط) بناء رابط الصورة الحقيقي
            // ---------------------------------------------------------

            var fileUrl =
                $"{Request.Scheme}://{Request.Host}/uploads/{uniqueFileName}";


            // ---------------------------------------------------------
            // ي) البحث عن الطالب وتحديث رابط الصورة
            // ---------------------------------------------------------

            var student =
                await unitOfWork.Students.FindAsync(
                    s => s.Name.ToLower() == username.ToLower()
                );


            if (student != null)
            {
                student.ProfilePictureUrl = fileUrl;

                await unitOfWork.CompleteAsync();
            }


            // ---------------------------------------------------------
            // ك) إرجاع النتيجة
            // ---------------------------------------------------------

            return Ok(new
            {
                Message = "تم رفع الصورة بنجاح.",

                ProfilePictureUrl = fileUrl
            });
        }
    }
}

