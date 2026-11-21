using System.ComponentModel.DataAnnotations;

namespace SmartCampusServicePortal.Models
{
    // A campus service / maintenance request raised by a student
    public class ServiceRequest
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Student name is required")]
        [Display(Name = "Student Name")]
        [StringLength(100)]
        public string StudentName { get; set; } = string.Empty;

        [Display(Name = "Roll Number")]
        [StringLength(20)]
        public string? StudentRollNo { get; set; }

        [Required(ErrorMessage = "Please choose a service category")]
        [Display(Name = "Service Category")]
        [StringLength(50)]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please describe the issue")]
        [Display(Name = "Issue Description")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Submitted On")]
        public DateTime SubmissionDate { get; set; } = DateTime.Now;

        [StringLength(20)]
        public string Status { get; set; } = "Pending";

        [Display(Name = "Admin Remarks")]
        [StringLength(500)]
        public string? AdminRemarks { get; set; }
    }
}
