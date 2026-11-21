using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartCampusServicePortal.Data;
using SmartCampusServicePortal.Models;

namespace SmartCampusServicePortal.Controllers
{
    // MVC controller for submitting / viewing / editing service requests
    public class ServiceRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServiceRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ServiceRequests
        public async Task<IActionResult> Index()
        {
            var list = await _context.ServiceRequests
                .OrderByDescending(r => r.SubmissionDate)
                .ToListAsync();
            return View(list);
        }

        // GET: ServiceRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var request = await _context.ServiceRequests.FirstOrDefaultAsync(r => r.Id == id);
            if (request == null) return NotFound();
            return View(request);
        }

        // GET: ServiceRequests/Create
        public async Task<IActionResult> Create()
        {
            await LoadStudentsAsync();
            return View();
        }

        // POST: ServiceRequests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("StudentName,StudentRollNo,Category,Description")] ServiceRequest request)
        {
            if (ModelState.IsValid)
            {
                // Auto-fill roll number from the registered student if left empty
                if (string.IsNullOrWhiteSpace(request.StudentRollNo))
                {
                    var student = await _context.Students
                        .FirstOrDefaultAsync(s => s.FullName == request.StudentName);
                    request.StudentRollNo = student?.StudentIdNumber;
                }

                request.SubmissionDate = DateTime.Now;
                request.Status = "Pending";

                _context.ServiceRequests.Add(request);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Request #{request.Id} submitted successfully!";
                return RedirectToAction(nameof(Index));
            }

            await LoadStudentsAsync();
            return View(request);
        }

        // GET: ServiceRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var request = await _context.ServiceRequests.FindAsync(id);
            if (request == null) return NotFound();
            return View(request);
        }

        // POST: ServiceRequests/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,StudentName,StudentRollNo,Category,Description")] ServiceRequest input)
        {
            if (id != input.Id) return NotFound();

            var existing = await _context.ServiceRequests.FindAsync(id);
            if (existing == null) return NotFound();

            if (ModelState.IsValid)
            {
                existing.StudentName = input.StudentName;
                existing.StudentRollNo = input.StudentRollNo;
                existing.Category = input.Category;
                existing.Description = input.Description;
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Request #{id} updated.";
                return RedirectToAction(nameof(Details), new { id });
            }
            return View(input);
        }

        // GET: ServiceRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var request = await _context.ServiceRequests.FirstOrDefaultAsync(r => r.Id == id);
            if (request == null) return NotFound();
            return View(request);
        }

        // POST: ServiceRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var request = await _context.ServiceRequests.FindAsync(id);
            if (request != null)
            {
                _context.ServiceRequests.Remove(request);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Request #{id} deleted.";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: ServiceRequests/Track?ticketId=1
        public async Task<IActionResult> Track(int? ticketId)
        {
            if (ticketId == null) return View(null);

            var request = await _context.ServiceRequests.FirstOrDefaultAsync(r => r.Id == ticketId);
            if (request == null)
            {
                ViewBag.ErrorMessage = $"No request found with ticket number #{ticketId}.";
            }
            return View(request);
        }

        private async Task LoadStudentsAsync()
        {
            ViewBag.Students = await _context.Students
                .OrderBy(s => s.FullName)
                .Select(s => new SelectListItem { Text = s.FullName, Value = s.StudentIdNumber })
                .ToListAsync();
        }
    }
}
