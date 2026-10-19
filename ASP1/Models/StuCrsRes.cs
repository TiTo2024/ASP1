namespace ASP1.Models
{
    public class StuCrsRes
    {
        public int StudentId { get; set; }
        public Student? Student { get; set; }

        public int CourseId { get; set; }
        public Course? Course { get; set; }

        public double Grade { get; set; }
    }
}
