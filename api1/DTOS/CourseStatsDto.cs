namespace api1.DTOS
{
    public class CourseStatsDto
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; } = string.Empty;
        public int TotalEnrolledStudents { get; set; }
        public double AverageGrade { get; set; }
        public double MaxGrade { get; set; }
        public double MinGrade { get; set; }
    }
}