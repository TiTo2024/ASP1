namespace ASP1.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }

        // Foreign Key
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }

        // Many-to-Many Join Entity
        public ICollection<StuCrsRes> StuCrsResults { get; set; } = new List<StuCrsRes>();
    }
}
