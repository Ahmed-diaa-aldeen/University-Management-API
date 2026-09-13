using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api1.Models
{
    public class Student
    {
        internal string name;

        // المعرف الرئيسي (Primary Key)
        [Key]
        public int Id { get; set; }

        // الخصائص الأساسية (Basic Properties)
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public int Age { get; set; }

        // أنواع البيانات المختلفة (Different Data Types)
        public DateTime BirthDate { get; set; }

        public decimal GPA { get; set; } // المعدل التراكمي

        public bool IsActive { get; set; } // هل الطالب مستمر أم متخرج/موقوف

        public string Gender { get; set; }

        public string Address { get; set; }

        // تاريخ الإضافة للنظام (مع قيمة افتراضية للتاريخ الحالي)
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // خاصية محسوبة (Read-only Computed Property) لا تُخزن في قاعدة البيانات
        [NotMapped]
        public bool IsPassed => GPA >= 2.0m;

        // دالة استاتيكية لتوليد قائمة بيانات عشوائية للاختبار
        public static List<Student> GetSampleStudents()
        {
            return new List<Student>
            {
                new Student
                {
                    Id = 1,
                    Name = "أحمد محمود العلي",
                    Email = "ahmed.ali@example.com",
                    Phone = "01012345678",
                    Age = 21,
                    BirthDate = new DateTime(2003, 5, 15),
                    GPA = 3.65m,
                    IsActive = true,
                    Gender = "Male",
                    Address = "القاهرة - Nasr City",
                    CreatedAt = DateTime.Now.AddDays(-100)
                },
                new Student
                {
                    Id = 2,
                    Name = "سارة إبراهيم حسن",
                    Email = "sara.ibrahim@example.com",
                    Phone = "01198765432",
                    Age = 22,
                    BirthDate = new DateTime(2002, 11, 20),
                    GPA = 3.90m,
                    IsActive = true,
                    Gender = "Female",
                    Address = "الجيزة - Maadi",
                    CreatedAt = DateTime.Now.AddDays(-80)
                },
                new Student
                {
                    Id = 3,
                    Name = "عمر خالد يوسف",
                    Email = "omar.khalid@example.com",
                    Phone = "01234567890",
                    Age = 20,
                    BirthDate = new DateTime(2004, 2, 10),
                    GPA = 1.85m,
                    IsActive = false,
                    Gender = "Male",
                    Address = "الإسكندرية - Smouha",
                    CreatedAt = DateTime.Now.AddDays(-45)
                },
                new Student
                {
                    Id = 4,
                    Name = "مريم محمد رضوان",
                    Email = "maryam.radwan@example.com",
                    Phone = "01511223344",
                    Age = 23,
                    BirthDate = new DateTime(2001, 8, 5),
                    GPA = 2.75m,
                    IsActive = true,
                    Gender = "Female",
                    Address = "المنصورة - Downtown",
                    CreatedAt = DateTime.Now.AddDays(-30)
                },
                new Student
                {
                    Id = 5,
                    Name = "يوسف طارق صلاح",
                    Email = "youssef.tarek@example.com",
                    Phone = "01099887766",
                    Age = 21,
                    BirthDate = new DateTime(2003, 1, 30),
                    GPA = 1.40m,
                    IsActive = true,
                    Gender = "Male",
                    Address = "طنطا - El-Gharbia",
                    CreatedAt = DateTime.Now.AddDays(-10)
                }
            };
        }
    }
}