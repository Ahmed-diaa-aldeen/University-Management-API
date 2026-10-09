using Microsoft.EntityFrameworkCore;

namespace api1.Models
{
    public class MVCContext : DbContext
    {
        public MVCContext(DbContextOptions<MVCContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }

        // خاصية برمجية فقط لتلبية استدعاءات db.Student القديمة بدون إنشاء جدول جديد
        public DbSet<Student> Student => Students;

        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=WINDOWS-PUC813S\\SQLEXPRESS;Database=UniversityDB_FinalFix;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // إجبار EF Core على ربط الكيان بجدول Students الوحيد لتجنب خطأ التكرار
            modelBuilder.Entity<Student>().ToTable("Students");

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentId);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId);
        }
    }
}