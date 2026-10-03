using api1.Models;

namespace api1.Repository
{
    public class studentRepository
    {
        MVCContext db;
        public studentRepository(MVCContext db)
        {
            this.db = db;
        }

        public List<Student> selectAll()
        {
            return db.Student.ToList();
        }

        public Student selectByid(int id)
        {
            return db.Student.Find(id);
        }

        public void add(Student s)
        {
            db.Student.Add(s);
            db.SaveChanges();
        }
        public void edit(Student s)
        {
            db.Entry(s).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
        }

        public void remove(int id)
        {
         Student s=db.Student.Find(id);
         db.Student.Remove(s);
            db.SaveChanges();
        }

        internal void saveChange()
        {
            throw new NotImplementedException();
        }
    }
}
