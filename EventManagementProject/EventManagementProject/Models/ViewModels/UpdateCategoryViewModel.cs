using System.ComponentModel.DataAnnotations;

namespace EventManagementProject.Models.ViewModels
{
    public class UpdateCategoryViewModel
    {
        [Required(ErrorMessage = "Please enter the category name.")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "The category name must be between 1 and 255 characters.")]
        public string CategoryName { get; set; }
        public int CategoryID { get; set; }
    }
}
