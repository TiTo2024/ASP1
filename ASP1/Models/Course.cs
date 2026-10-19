namespace ASP1.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Degree { get; set; }
        public double MinDegree { get; set; }

        // Foreign Key
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }

        // Navigation Properties
        public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
        public ICollection<StuCrsRes> StuCrsResults { get; set; } = new List<StuCrsRes>();
    }
}
