using api1.Models;
using Microsoft.EntityFrameworkCore;

namespace api1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // ????? ??? DbContext ?????? ??????? ??? ????? ???? ????
            builder.Services.AddDbContext<MVCContext>(options =>
                options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=StudentDB;Trusted_Connection=True;TrustServerCertificate=True;"));

            var app = builder.Build();
            

            // السطر التالي يتأكد من إنشاء قاعدة البيانات والجدول تلقائياً إذا لم تكن موجودة
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<MVCContext>();
                dbContext.Database.EnsureCreated();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}