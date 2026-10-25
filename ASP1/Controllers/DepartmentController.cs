using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASP1.Data;
using ASP1.Models;
using ASP1.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASP1.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepartmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. ShowAll Lifecycle
        public IActionResult ShowAll()
        {
            var departments = _context.Departments
                .Include(d => d.Students)
                .ToList();
            return View(departments);
        }

        // 2. ShowDetails Lifecycle
        public IActionResult ShowDetails(int id)
        {
            var department = _context.Departments
                .Include(d => d.Students)
                .Include(d => d.Courses)
                .Include(d => d.Teachers)
                .FirstOrDefault(d => d.Id == id);

            if (department == null)
            {
                return NotFound("Department not found.");
            }

            return View(department);
        }

        // 3. Add (GET) Lifecycle
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        // 4. Add (POST) Lifecycle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Departments.Add(department);
                _context.SaveChanges();
                return RedirectToAction(nameof(ShowAll));
            }
            return View(department);
        }

        // 5. Department Report using ViewModel (Students > 25 and Sizing State)
        public IActionResult DepartmentReport(int id)
        {
            var department = _context.Departments
                .Include(d => d.Students)
                .FirstOrDefault(d => d.Id == id);

            if (department == null)
            {
                return NotFound("Department not found.");
            }

            int studentCount = department.Students.Count;
            string state = studentCount > 50 ? "Main" : "Branch";

            var filteredStudents = department.Students
                .Where(s => s.Age > 25)
                .ToList();

            var viewModel = new DepartmentDetailsViewModel
            {
                DepartmentName = department.Name,
                DepartmentState = state,
                StudentsOver25List = new SelectList(filteredStudents, "Id", "Name")
            };

            return View(viewModel);
        }
    }
}