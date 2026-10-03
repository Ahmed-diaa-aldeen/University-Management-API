namespace Project2.DTO
{
    public class DepDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public string location { get; set; }
        public List<string> studentsnames { get; set; }=new List<string>();
        

    }
}
