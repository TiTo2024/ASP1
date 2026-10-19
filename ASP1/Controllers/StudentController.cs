using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASP1.Data;

namespace ASP1.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // URL: /student/ShowAll
        public IActionResult ShowAll()
        {
            var students = _context.Students
                .Include(s => s.Department)
                .ToList();

            return View(students);
        }

        // URL: /Student/ShowDetails?id=3
        public IActionResult ShowDetails(int id)
        {
            var student = _context.Students
                .Include(s => s.Department)
                .Include(s => s.StuCrsResults)
                    .ThenInclude(sc => sc.Course)
                .FirstOrDefault(s => s.Id == id);

            if (student == null)
            {
                return NotFound("Student not found.");
            }

            return View(student);
        }
    }
}