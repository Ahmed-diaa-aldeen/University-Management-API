using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project2.Models
{
    [Table("Department")]

    public class Department
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public virtual ICollection<Student> Students { get; set; }
            = new List<Student>();
    }
}