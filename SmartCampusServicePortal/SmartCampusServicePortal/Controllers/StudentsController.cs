using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartCampusServicePortal.Data;
using SmartCampusServicePortal.Models;

namespace SmartCampusServicePortal.Controllers
{
    // Student Controller - Handles student registration and profiles
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            var students = await _context.Students
                .OrderByDescending(s => s.RegisteredDate)
                .ToListAsync();
            return View(students);
        }

        // GET: Students/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Students/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Student student)
        {
            if (ModelState.IsValid)
            {
                bool idExists = await _context.Students.AnyAsync(s => s.StudentIdNumber == student.StudentIdNumber);
                if (idExists)
                {
                    ModelState.AddModelError("StudentIdNumber", "A student with this ID / Roll number already exists.");
                    return View(student);
                }

                student.RegisteredDate = DateTime.Now;
                _context.Students.Add(student);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Student '{student.FullName}' registered successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View(student);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == id);
            if (student == null) return NotFound();

            ViewBag.Requests = await _context.ServiceRequests
                .Where(r => r.StudentRollNo == student.StudentIdNumber || r.StudentName == student.FullName)
                .OrderByDescending(r => r.SubmissionDate)
                .ToListAsync();

            return View(student);
        }

        // GET: Students/Dashboard?rollNo=STU101
        public async Task<IActionResult> Dashboard(string? rollNo)
        {
            if (string.IsNullOrWhiteSpace(rollNo)) return View();

            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.StudentIdNumber == rollNo.Trim());
            if (student == null)
            {
                ViewBag.ErrorMessage = $"No student found with Roll Number '{rollNo}'.";
                return View();
            }

            ViewBag.Student = student;
            ViewBag.Requests = await _context.ServiceRequests
                .Where(r => r.StudentRollNo == student.StudentIdNumber || r.StudentName == student.FullName)
                .OrderByDescending(r => r.SubmissionDate)
                .ToListAsync();
            return View();
        }
    }
}
