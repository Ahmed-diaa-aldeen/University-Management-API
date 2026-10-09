namespace api1.Models
{
    public class Enrollment
    {
        public int Id { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;

        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        // --- الحقول الجديدة للدرجات ---

        // درجة Midterm (مثلاً من 40)
        public double MidtermScore { get; set; }

        // درجة Final (مثلاً من 60)
        public double FinalScore { get; set; }

        // إجمالي الدرجة (حقل محسوب: Midterm + Final)
        public double TotalScore => MidtermScore + FinalScore;

        // التقدير الحرفي للمادة (مثل: A, B, C, D, F)
        public string Grade { get; set; } = "N/A";


    }
}