using System.ComponentModel.DataAnnotations;

namespace EventManagementProject.Models
{
    public class EventCategory
    {
        [Key]
        public int CategoryID { get; set; }

        [StringLength(255, MinimumLength = 0)]
        public string CategoryName { get; set; }  
        // Quan hệ với Event: Một Category có thể có nhiều Event
        public ICollection<Event> Events { get; set; }
    }
}
