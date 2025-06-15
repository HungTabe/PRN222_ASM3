using System.ComponentModel.DataAnnotations;

namespace EventManagementProject.Models.ViewModels
{
    public class EditEventViewModel
    {
        public int EventID { get; set; }

        [Required(ErrorMessage = "Please enter the event title.")]
        [StringLength(255, ErrorMessage = "Title cannot exceed 255 characters.")]
        public string Title { get; set; }

        [StringLength(int.MaxValue, ErrorMessage = "Description is too long.")]
        public string? Description { get; set; }

        [StringLength(255, ErrorMessage = "Location cannot exceed 255 characters.")]
        public string? Location { get; set; }

        [Required(ErrorMessage = "Please select a start time.")]
        public DateTime? StartTime { get; set; }

        [Required(ErrorMessage = "Please select an end time.")]
        public DateTime? EndTime { get; set; }

        [Required(ErrorMessage = "Please select a category.")]
        public int? CategoryID { get; set; }
    }
}
