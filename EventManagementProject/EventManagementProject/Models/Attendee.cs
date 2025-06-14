using System.ComponentModel.DataAnnotations;

namespace EventManagementProject.Models
{
    public class Attendee
    {
        [Key]
        public int AttendeeID { get; set; }      
        public int? UserID { get; set; }           
        public int? EventID { get; set; }

        [StringLength(255, MinimumLength = 0)]
        public string? Name { get; set; }

        [StringLength(255, MinimumLength = 0)]
        public string? Email { get; set; }         
        public DateTime? RegistrationTime { get; set; }  

        // Quan hệ với User: Một Attendee thuộc về một User
        public User User { get; set; }
        // Quan hệ với Event: Một Attendee thuộc về một Event
        public Event Event { get; set; }
    }
}
