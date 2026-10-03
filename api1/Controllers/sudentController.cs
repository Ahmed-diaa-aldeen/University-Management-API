using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using api1.Models;
using api1.Repository;

namespace api1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class studentController : ControllerBase
    {
        private readonly studentRepository _studentRepo;

        // حقن الـ Repository داخل الكنترولر
        public studentController(studentRepository studentRepo)
        {
            _studentRepo = studentRepo;
        }

        // GET: api/student
        [HttpGet]
        public IActionResult GetAll()
        {
            var students = _studentRepo.selectAll();
            return Ok(students);
        }

        // GET: api/student/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var student = _studentRepo.selectByid(id);
            if (student == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }
            return Ok(student);
        }

        // POST: api/student
        [HttpPost]
        public IActionResult Add(Student student)
        {
            if (student == null)
            {
                return BadRequest();
            }

            _studentRepo.add(student);
            return Ok(student);
        }

        // PUT: api/student/5
        [HttpPut("{id}")]
        public IActionResult Edit(int id, Student student)
        {
            if (id != student.Id) // افترضنا أن الـ Primary Key اسمه Id
            {
                return BadRequest("ID mismatch.");
            }

            var existingStudent = _studentRepo.selectByid(id);
            if (existingStudent == null)
            {
                return NotFound();
            }

            _studentRepo.edit(student);
            return NoContent();
        }

        // DELETE: api/student/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var student = _studentRepo.selectByid(id);
            if (student == null)
            {
                return NotFound();
            }

            _studentRepo.remove(id);
            return Ok("Deleted successfully.");
        }

        public void save()
        {
            _studentRepo.saveChange();

        }
    }
}