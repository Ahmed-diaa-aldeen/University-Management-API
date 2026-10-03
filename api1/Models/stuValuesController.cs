using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using api1.Repository;

namespace api1.Models
{
    [Route("api/[controller]")]
    [ApiController]
    public class stuValuesController : ControllerBase
    {
        studentRepository rep;
        MVCContext db;
        public stuValuesController(MVCContext db)
        {
            this.db = db;
        }
        [HttpGet]
        public List <Student> getall()
        {
            return db.Student.ToList();
        }

        //id 
        [HttpGet("{id}")]
        public ActionResult getbyid(int id)
        {
            Student s= db.Student.Find(id);
            if(s == null)
            
                return NotFound();
                else
                return Ok(s);
            
        }


        //Name 
        [HttpGet("/api/sts/{name}")]
        public ActionResult getbyname(string name )
        {
            Student s = db.Student.FirstOrDefault(n=>n.name == name);
            if (s == null)

                return NotFound();
            else
                return Ok(s);

        }

        //add
        [HttpPost]
        public ActionResult add(Student s)
        {
            if(s == null) return BadRequest();
            if(!ModelState.IsValid) return BadRequest();
            db.Student.Add(s);
            db.SaveChanges();
           // return Created("Ahmed ", s);
           return CreatedAtAction ("Getbyid",new {id =s.Id},s);
        }

        
        //edit
        [HttpPut("{id}")]
        public ActionResult edit(int id, [FromBody] Student s)
        {
            if (s == null) return BadRequest();

            // التعديل هنا: استخدام != للمقارنة الصحيحة
            if (s.Id != id) return BadRequest();

            db.Entry(s).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!db.Student.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // برجع 204 عند نجاح التعديل
        }
        [HttpDelete("{id}")]
        public ActionResult delete(int id)
        {
            Student s = db.Student.Find(id);
            if (s == null) return BadRequest();
            db.Student.Remove(s);
            db.SaveChanges();
            return Ok();
        }

    }
}
