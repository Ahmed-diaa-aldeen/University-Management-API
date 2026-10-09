using System.Security.Claims;
using api1.DTOS;
using api1.Models;
using api1.Repository.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // قصر الوصول للـ Endpoints داخل هذا الـ Controller على المستخدمين المحققين الذين يحملون دور Student أو Admin
    [Authorize(Roles = "Student,Admin")]
    public class ProfileController(IUnitOfWork unitOfWork) : ControllerBase
    {
        /// <summary>
        /// 1. إرجاع البروفايل الخاص بالمستخدم الحالي المسجل في الـ Token
        /// </summary>
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            // استخراج اسم المستخدم تلقائياً من الـ ClaimTypes.Name المضمن في الـ JWT Token لضمان الأمان
            var username = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(username))
                return Unauthorized("التوكن غير صالح أو منتهي الصلاحية.");

            // بحث مباشر واستعلام سريع عبر الـ Repository للبحث بـ Expression بدون تحميل كل البيانات
            var student = await unitOfWork.Students.FindAsync(s => s.Name.ToLower() == username.ToLower());

            // في حالة التسجيل لأول مرة، إنشاء سجّل طالب افتراضي ربطاً برمز المستخدم
            if (student == null)
            {
                student = new Student
                {
                    Name = username,
                    Email = $"{username.Replace(" ", "").ToLower()}@university.com",
                    AcademicId = "2026" + Random.Shared.Next(1000, 9999),
                    NationalId = "30000000000000",
                    AcademicYear = 1,
                    Department = "General",
                    GPA = 0.0,
                    ProfilePictureUrl = ""
                };

                await unitOfWork.Students.AddAsync(student);
                await unitOfWork.CompleteAsync(); // حفظ التغييرات فوراً لتوليد الـ Primary Key
            }

            // تحويل الـ Domain Model إلى DTO لحماية الحقول الحساسة وعدم إرجاع بيانات زائدة للـ Client
            var profileDto = new StudentProfileDto
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                AcademicId = student.AcademicId,
                NationalId = student.NationalId,
                AcademicYear = student.AcademicYear,
                Department = student.Department,
                GPA = student.GPA,
                ProfilePictureUrl = student.ProfilePictureUrl
            };

            return Ok(profileDto);
        }

        /// <summary>
        /// 2. تحديث البيانات الشخصية البسيطة (الإيميل/الصورة) للبروفايل الحساب الحالي
        /// </summary>
        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateProfileDto dto)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(username))
                return Unauthorized("التوكن غير صالح أو منتهي الصلاحية.");

            var student = await unitOfWork.Students.FindAsync(s => s.Name.ToLower() == username.ToLower());

            if (student == null)
            {
                student = new Student
                {
                    Name = username,
                    Email = dto.Email,
                    AcademicId = "2026" + Random.Shared.Next(1000, 9999),
                    NationalId = "30000000000000",
                    AcademicYear = 1,
                    Department = "General",
                    GPA = 0.0,
                    ProfilePictureUrl = dto.ProfilePictureUrl ?? ""
                };

                await unitOfWork.Students.AddAsync(student);
            }
            else
            {
                // تحديث الحقول المسموح للعميل بتغييرها فقط لحماية بقية البيانات مثل GPA أو AcademicId
                student.Email = dto.Email;
                if (!string.IsNullOrEmpty(dto.ProfilePictureUrl))
                    student.ProfilePictureUrl = dto.ProfilePictureUrl;
            }

            await unitOfWork.CompleteAsync();
            return Ok("تم تحديث بيانات البروفايل بنجاح.");
        }
    }
}