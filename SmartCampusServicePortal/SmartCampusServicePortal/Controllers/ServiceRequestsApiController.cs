using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartCampusServicePortal.Data;
using SmartCampusServicePortal.Models;

namespace SmartCampusServicePortal.Controllers
{
    // REST API Controller exposing endpoints for Service Requests
    [Route("api/servicerequests")]
    [ApiController]
    public class ServiceRequestsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ServiceRequestsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/servicerequests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceRequest>>> GetAllRequests()
        {
            var requests = await _context.ServiceRequests
                .OrderByDescending(r => r.SubmissionDate)
                .ToListAsync();
            return Ok(requests);
        }

        // GET: api/servicerequests/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceRequest>> GetRequestById(int id)
        {
            var request = await _context.ServiceRequests.FindAsync(id);
            if (request == null)
            {
                return NotFound(new { message = $"Service request with ID {id} was not found." });
            }
            return Ok(request);
        }

        // POST: api/servicerequests
        [HttpPost]
        public async Task<ActionResult<ServiceRequest>> CreateRequest([FromBody] ServiceRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            request.Id = 0; // let the database generate the ID
            if (request.SubmissionDate == default)
            {
                request.SubmissionDate = DateTime.Now;
            }
            if (string.IsNullOrWhiteSpace(request.Status))
            {
                request.Status = "Pending";
            }

            _context.ServiceRequests.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRequestById), new { id = request.Id }, request);
        }

        // PUT: api/servicerequests/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRequest(int id, [FromBody] ServiceRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest(new { message = "Request ID mismatch." });
            }

            var existingRequest = await _context.ServiceRequests.FindAsync(id);
            if (existingRequest == null)
            {
                return NotFound(new { message = $"Service request with ID {id} not found." });
            }

            existingRequest.StudentName = request.StudentName;
            existingRequest.StudentRollNo = request.StudentRollNo;
            existingRequest.Category = request.Category;
            existingRequest.Description = request.Description;
            existingRequest.Status = request.Status;
            existingRequest.AdminRemarks = request.AdminRemarks;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Service request updated successfully.", data = existingRequest });
        }

        // PUT: api/servicerequests/{id}/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] StatusUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingRequest = await _context.ServiceRequests.FindAsync(id);
            if (existingRequest == null)
            {
                return NotFound(new { message = $"Service request with ID {id} not found." });
            }

            existingRequest.Status = dto.Status;
            if (dto.AdminRemarks != null)
            {
                existingRequest.AdminRemarks = dto.AdminRemarks;
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Request status updated successfully.", data = existingRequest });
        }

        // DELETE: api/servicerequests/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var request = await _context.ServiceRequests.FindAsync(id);
            if (request == null)
            {
                return NotFound(new { message = $"Service request with ID {id} not found." });
            }

            _context.ServiceRequests.Remove(request);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Service request deleted successfully." });
        }
    }
}
