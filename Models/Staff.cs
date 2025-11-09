using System;
using System.ComponentModel.DataAnnotations;

namespace HRDepartment.Models
{
    public class Staff
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Staff ID")]
        public string StaffId { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Email")]
        [RegularExpression(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$", ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Phone")]
        [RegularExpression(@"^\+?\d{1,4}?[-.\s]?\(?\d{1,3}?\)?[-.\s]?\d{1,4}[-.\s]?\d{1,4}[-.\s]?\d{1,9}$", ErrorMessage = "Invalid phone number")]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Starting Date")]
        public DateTime StartingDate { get; set; }

        // Path relative to wwwroot (e.g. /uploads/abc.jpg)
        [Display(Name = "Photo")]
        public string? PhotoPath { get; set; }
    }
}
