using System.ComponentModel.DataAnnotations;

namespace EventManagementProject.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Please enter a username.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter a password.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter your full name.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Please enter an email address.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }
    }
}
