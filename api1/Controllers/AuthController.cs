using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using api1.DTOS;

namespace api1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDto dto)
        {
            // إنشاء الـ Token فور التسجيل وإدراج دور المستخدم (Role) الممرر في DTO
            var token = GenerateJwtToken(dto.Username, dto.Role);
            return Ok(new AuthResponseDto
            {
                Token = token,
                Username = dto.Username,
                Role = dto.Role
            });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            // فحص تجريبي: تحديد الدور (Admin / Student) بناءً على اسم المستخدم لغرض الاختبار
            string role = dto.Username.ToLower().Contains("admin") ? "Admin" : "Student";

            var token = GenerateJwtToken(dto.Username, role);
            return Ok(new AuthResponseDto
            {
                Token = token,
                Username = dto.Username,
                Role = role
            });
        }

        /// <summary>
        /// دالة توليد رمز الـ JWT وتضمين الهوية (Claims) والصلاحيات (Roles)
        /// </summary>
        private string GenerateJwtToken(string username, string role)
        {
            // 1. تحويل المفتاح السري المكتوب في appsettings.json إلى SymmetricSecurityKey للتحقق من سلامة الـ Token
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

            // 2. تحديد خوارزمية التوقيع الرقمي (HmacSha256) لمنع التلاعب في بيانات الـ Token
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // 3. إعداد الـ Claims (بيانات الهوية والـ Roles):
            // ClaimTypes.Name : يُستخرج لاحقاً لمعرفة اسم المستخدم الحالي.
            // ClaimTypes.Role : يُستخدم بواسطة [Authorize(Roles = "...")] لمعرفة صلاحيات المستخدم.
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };

            // 4. بناء كائن הـ JWT وتحديد مدة الصلاحية والمعلمات الأساسية
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(3), // تنتهي صلاحية التوكن بعد 3 ساعات
                signingCredentials: credentials);

            // 5. كتابة الـ Token في شكل String مشفر مفرغ ليرسل إلى العميل (Client)
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}