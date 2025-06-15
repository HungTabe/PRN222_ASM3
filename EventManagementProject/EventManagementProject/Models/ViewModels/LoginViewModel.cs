using System.ComponentModel.DataAnnotations;

namespace EventManagementProject.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please enter a username.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter a password.")]
        public string Password { get; set; }
    }
}
