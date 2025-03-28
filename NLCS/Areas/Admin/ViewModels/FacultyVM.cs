using System.ComponentModel.DataAnnotations;

namespace NLCS.Areas.Admin.ViewModels
{
    public class FacultyVM
    {
        [Required(ErrorMessage = "*")]
        [RegularExpression(@"^[A-Z]{2}\d{3}$", ErrorMessage = "ID phải có định dạng: 2 chữ cái hoa và 3 chữ số.")]
        public string ID { get; set; }

        [Required(ErrorMessage = "*")]
        public string Name { get; set; }

        [Required(ErrorMessage = "*")]
        public string Description { get; set; }

        [Required(ErrorMessage = "*")]
        public IFormFile Image { get; set; }
        public string ImageUrl { get; set; } = "/";
    }
}
