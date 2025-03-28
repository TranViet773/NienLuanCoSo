using System.ComponentModel.DataAnnotations;

namespace NLCS.Areas.Dean.ViewModels
{
    public class EditTeacherVM
    {
        public string Teacher_Name { get; set; }
        public string Teacher_Email { get; set; }
        public string Teacher_Username { get; set; }
        public string Teacher_Password { get; set; }
        public string Teacher_Degree { get; set; }
        public IFormFile? Teacher_Image { get; set; }
        public string Url_Image { get; set; } = "/";
        public string Teacher_PhoneNumber { get; set; }
        public bool IsAdmin { get; set; } = false; // Mặc định là false
        public bool isActive { get; set; } = true; // Mặc định là false
    }
}
