using System.ComponentModel.DataAnnotations;

namespace EventManagementProject.Models
{
    public class Event
    {
        [Key]
        public int EventID { get; set; }          
        public string Title { get; set; }

        [StringLength(int.MaxValue, MinimumLength = 0)]
        public string? Description { get; set; }

        [StringLength(255, MinimumLength = 0)]
        public string? Location { get; set; }
        public DateTime? StartTime { get; set; }   
        public DateTime? EndTime { get; set; }     
        public int? CategoryID { get; set; }       

        // Quan hệ với EventCategory: Một Event thuộc về một Category
        public EventCategory EventCategory { get; set; }
        // Quan hệ với Attendee: Một Event có thể có nhiều Attendee
        public ICollection<Attendee> Attendees { get; set; }
    }
}
