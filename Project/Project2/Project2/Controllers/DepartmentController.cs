using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project2.DTO;
using Project2.Models;

namespace Project2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly diaaStudentDBContext db;

        public DepartmentController(diaaStudentDBContext context)
        {
            db = context;
        }

        [HttpGet]
        public ActionResult gatall()
        {
            List<Department> deps = db.Departments.ToList();
            List<DepDto> depDtos = new List<DepDto>();

            foreach (Department dept in deps)
            {
                DepDto depDto = new DepDto()
                {
                    id = dept.Id,
                    name = dept.Name,
                    location = dept.Location,
                    studentsnames = dept.Students
                        .Select(s => s.Name)
                        .ToList()
                };
                depDtos.Add(depDto);

            }
            return Ok(depDtos);
        }

            [HttpGet("{id}")]
        public ActionResult getbyid(int id)
        {
            Department d = db.Departments
                .Include(x => x.Students)
                .FirstOrDefault(x => x.Id == id);

            if (d == null)
            {
                return NotFound();
            }

            DepDto depDto = new DepDto()
            {
                id = d.Id,
                name = d.Name,
                location = d.Location,
                studentsnames = d.Students
                    .Select(s => s.Name)
                    .ToList()
            };

            return Ok(depDto);
        }
    }
}