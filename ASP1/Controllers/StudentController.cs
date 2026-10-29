using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASP1.Data;
using ASP1.Models;

namespace ASP1.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. ShowAll Lifecycle: Supports Pagination, Search by Name, and Filter by Department
        public async Task<IActionResult> ShowAll(string searchString, int? departmentId, int pageNumber = 1)
        {
            int pageSize = 5; // عدد الطلاب في كل صفحة

            var studentsQuery = _context.Students
                .Include(s => s.Department)
                .AsQueryable();

            // تطبيق البحث بالاسم
            if (!string.IsNullOrEmpty(searchString))
            {
                studentsQuery = studentsQuery.Where(s => s.Name.Contains(searchString));
            }

            // تطبيق الفلترة بالقسم
            if (departmentId.HasValue && departmentId.Value > 0)
            {
                studentsQuery = studentsQuery.Where(s => s.DepartmentId == departmentId.Value);
            }

            // تمرير الأقسام للـ View عشان تظهر في قائمة الفلتر
            ViewBag.Departments = new SelectList(_context.Departments, "Id", "Name", departmentId);
            ViewBag.CurrentSearch = searchString;
            ViewBag.SelectedDepartment = departmentId;

            // تنفيذ الباجينيشن (Pagination)
            var paginatedList = await PaginatedList<Student>.CreateAsync(studentsQuery.AsNoTracking(), pageNumber, pageSize);

            return View(paginatedList);
        }

        // 2. ShowDetails Lifecycle (GetById)
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

        // 3. Add (GET) Lifecycle: Show Departments dropdown
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Departments = new SelectList(_context.Departments, "Id", "Name");
            return View();
        }

        // 4. Add (POST) Lifecycle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Students.Add(student);
                _context.SaveChanges();
                return RedirectToAction(nameof(ShowAll));
            }
            ViewBag.Departments = new SelectList(_context.Departments, "Id", "Name", student.DepartmentId);
            return View(student);
        }

        // 5. Edit (GET) Lifecycle: Load student and Departments dropdown
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var student = _context.Students.Find(id);
            if (student == null)
            {
                return NotFound();
            }
            ViewBag.Departments = new SelectList(_context.Departments, "Id", "Name", student.DepartmentId);
            return View(student);
        }

        // 6. Edit (POST) Lifecycle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Students.Any(e => e.Id == student.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(ShowAll));
            }
            ViewBag.Departments = new SelectList(_context.Departments, "Id", "Name", student.DepartmentId);
            return View(student);
        }

        // 7. Delete (GET) Lifecycle: Direct to warning confirmation View
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var student = _context.Students
                .Include(s => s.Department)
                .FirstOrDefault(s => s.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student); // شاشة التحذير (Warning View)
        }

        // 8. Delete (POST) Lifecycle: Confirm deletion
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var student = _context.Students.Find(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(ShowAll));
        }
    }
}