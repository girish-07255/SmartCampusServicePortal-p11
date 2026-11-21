using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartCampusServicePortal.Data;

namespace SmartCampusServicePortal.Controllers
{
    // Admin portal: filter requests and update their resolution status
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin?category=Hostel&status=Pending
        public async Task<IActionResult> Index(string? category, string? status)
        {
            var query = _context.ServiceRequests.AsQueryable();

            if (!string.IsNullOrWhiteSpace(category))
                query = query.Where(r => r.Category == category);
            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(r => r.Status == status);

            ViewBag.Category = category;
            ViewBag.Status = status;

            var list = await query.OrderByDescending(r => r.SubmissionDate).ToListAsync();
            return View(list);
        }

        // GET: Admin/UpdateStatus/4
        public async Task<IActionResult> UpdateStatus(int? id)
        {
            if (id == null) return NotFound();
            var request = await _context.ServiceRequests.FindAsync(id);
            if (request == null) return NotFound();
            return View(request);
        }

        // POST: Admin/UpdateStatus/4
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string status, string? adminRemarks)
        {
            var request = await _context.ServiceRequests.FindAsync(id);
            if (request == null) return NotFound();

            request.Status = status;
            request.AdminRemarks = adminRemarks;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Request #{id} marked as {status}.";
            return RedirectToAction(nameof(Index));
        }
    }
}
