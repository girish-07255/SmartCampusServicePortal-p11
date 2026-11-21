using System.ComponentModel.DataAnnotations;

namespace SmartCampusServicePortal.Models
{
    // Represents a student registered in the campus portal
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Student ID / Roll No is required")]
        [Display(Name = "Student ID / Roll No")]
        [StringLength(20, ErrorMessage = "ID cannot exceed 20 characters")]
        public string StudentIdNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Full Name is required")]
        [Display(Name = "Full Name")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "Email Address")]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        [Display(Name = "Phone Number")]
        [StringLength(15)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Department is required")]
        [Display(Name = "Department")]
        [StringLength(50)]
        public string Department { get; set; } = string.Empty;

        [Display(Name = "Registration Date")]
        public DateTime RegisteredDate { get; set; } = DateTime.Now;
    }
}
