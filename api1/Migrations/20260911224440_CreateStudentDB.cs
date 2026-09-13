using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api1.Migrations
{
    /// <inheritdoc />
    public partial class CreateStudentDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Student",
                columns: new[] { "Id", "Address", "Age", "BirthDate", "CreatedAt", "Email", "GPA", "Gender", "IsActive", "Name", "Phone" },
                values: new object[,]
                {
                    { 1, "القاهرة - Nasr City", 21, new DateTime(2003, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 4, 1, 44, 40, 125, DateTimeKind.Local).AddTicks(1823), "ahmed.ali@example.com", 3.65m, "Male", true, "أحمد محمود العلي", "01012345678" },
                    { 2, "الجيزة - Maadi", 22, new DateTime(2002, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 24, 1, 44, 40, 125, DateTimeKind.Local).AddTicks(1839), "sara.ibrahim@example.com", 3.90m, "Female", true, "سارة إبراهيم حسن", "01198765432" },
                    { 3, "الإسكندرية - Smouha", 20, new DateTime(2004, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 7, 29, 1, 44, 40, 125, DateTimeKind.Local).AddTicks(1844), "omar.khalid@example.com", 1.85m, "Male", false, "عمر خالد يوسف", "01234567890" },
                    { 4, "المنصورة - Downtown", 23, new DateTime(2001, 8, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 13, 1, 44, 40, 125, DateTimeKind.Local).AddTicks(1851), "maryam.radwan@example.com", 2.75m, "Female", true, "مريم محمد رضوان", "01511223344" },
                    { 5, "طنطا - El-Gharbia", 21, new DateTime(2003, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 2, 1, 44, 40, 125, DateTimeKind.Local).AddTicks(1857), "youssef.tarek@example.com", 1.40m, "Male", true, "يوسف طارق صلاح", "01099887766" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Student",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Student",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Student",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Student",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Student",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
