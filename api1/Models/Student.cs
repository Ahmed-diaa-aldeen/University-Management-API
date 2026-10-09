namespace api1.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // --- البيانات الجديدة للبروفايل الأكاديمي الشامل ---
        public string AcademicId { get; set; } = string.Empty;  // الرقم الأكاديمي (مثل 20261001)
        public string NationalId { get; set; } = string.Empty;  // الرقم القومي
        public int AcademicYear { get; set; } = 1;              // السنة الدراسية (1, 2, 3, 4)
        public string Department { get; set; } = string.Empty;  // القسم الأكاديمي
        public double GPA { get; set; } = 0.0;                  // التقدير التراكمي
        public string? ProfilePictureUrl { get; set; }          // رابط الصورة الشخصية

        // علاقة التسجيل بالمواد (Enrollments)
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}