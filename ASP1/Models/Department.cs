namespace ASP1.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string MgrName { get; set; } = string.Empty;

        // Navigation Properties
        public ICollection<Student> Students { get; set; } = new List<Student>();
        public ICollection<Course> Courses { get; set; } = new List<Course>();
        public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
    }
}
