namespace api1.DTOS
{
    // DTO لعرض بيانات البروفايل الكاملة
    public class StudentProfileDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string AcademicId { get; set; } = string.Empty;
        public string NationalId { get; set; } = string.Empty;
        public int AcademicYear { get; set; }
        public string Department { get; set; } = string.Empty;
        public double GPA { get; set; }
        public string? ProfilePictureUrl { get; set; }
    }

    // DTO للتعديل على البيانات المسموح للطالب بتغييرها فقط
    public class UpdateProfileDto
    {
        public string Email { get; set; } = string.Empty;
        public string? ProfilePictureUrl { get; set; }
    }
}