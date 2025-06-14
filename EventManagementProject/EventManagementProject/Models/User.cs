using System.ComponentModel.DataAnnotations;

namespace EventManagementProject.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }           
        public string Username { get; set; }      
        public string Password { get; set; }

        [StringLength(255, MinimumLength = 0)]
        public string? FullName { get; set; }

        [StringLength(255, MinimumLength = 0)]
        public string? Email { get; set; }

        [StringLength(50, MinimumLength = 0)]
        public string? Role { get; set; }          

        // Quan hệ với Attendee: Một User có thể có nhiều Attendee
        public ICollection<Attendee> Attendees { get; set; }
    }
}
