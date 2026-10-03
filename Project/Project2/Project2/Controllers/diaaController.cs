using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project2.DTO;
using Project2.Models;

namespace Project2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class diaaController : ControllerBase
    {
        private readonly diaaStudentDBContext db;

        public diaaController(diaaStudentDBContext context)
        {
            db = context;
        }

        // الطريقة الأولى: باستخدام foreach و StuDTO
        [HttpGet]
        public ActionResult getall()
        {
            List<Student> STS = db.Students.ToList();
            List<StuDTO> STSDTO = new List<StuDTO>();

            foreach (Student stu in STS)
            {
                StuDTO stdto = new StuDTO()
                {
                    Id = stu.Id,
                    fullname = stu.Name,
                    address = stu.Address,
                    age = stu.Age,
                    DepartmentName = stu.Name // عدّلها لـ stu.Department.Name عند إضافة علاقة القسم
                };
                STSDTO.Add(stdto);
            }

            return Ok(STSDTO);
        }

        [HttpGet("{id}")]
        public ActionResult getbyid(int id)
        {
            Student s = db.Students.Find(id);
            if (s == null) return NotFound();

            StuDTO stdto = new StuDTO()
            {
                Id = s.Id,
                fullname = s.Name,
                address = s.Address,
                age = s.Age,
                DepartmentName = s.Name
            };

            return Ok(stdto);
        }
    }
}