namespace api1.DTOS
{
    public class StudentDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public double GPA { get; set; }
        public List<StudentCourseDto> Courses { get; set; } = new List<StudentCourseDto>();
    }

    public class StudentCourseDto
    {
        public string CourseTitle { get; set; } = string.Empty;
        public int CreditHours { get; set; }
        public double Grade { get; set; }
    }
}
