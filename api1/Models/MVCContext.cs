#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using api1.Models;

namespace api1.Models;

public partial class MVCContext : DbContext
{
    public MVCContext()
    {
    }

    public MVCContext(DbContextOptions<MVCContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Student> Student { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=StudentDB;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>().ToTable("Student");

        // تحديد اسم الكلاس بالكامل يمنع الخطأ تماماً
        modelBuilder.Entity<Student>().HasData(api1.Models.Student.GetSampleStudents());

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}