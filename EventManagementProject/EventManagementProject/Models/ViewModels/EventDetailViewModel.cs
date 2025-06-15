namespace EventManagementProject.Models.ViewModels
{
    public class EventDetailViewModel
    {
        public int EventID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string CategoryName { get; set; }
        public int AttendeeCount { get; set; }
    }
}
