using System.ComponentModel.DataAnnotations;

namespace SmartCampusServicePortal.Models
{
    // Used by PUT api/servicerequests/{id}/status
    public class StatusUpdateDto
    {
        [Required]
        public string Status { get; set; } = "Pending";

        [StringLength(500)]
        public string? AdminRemarks { get; set; }
    }
}
