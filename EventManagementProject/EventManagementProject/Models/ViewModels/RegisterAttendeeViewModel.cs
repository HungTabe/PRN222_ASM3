using System.ComponentModel.DataAnnotations;

namespace EventManagementProject.Models.ViewModels
{
    public class RegisterAttendeeViewModel
    {
        [Required(ErrorMessage = "Please enter your name.")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 255 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter your email.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters.")]
        public string Email { get; set; }

        public int EventID { get; set; }
    }
}
