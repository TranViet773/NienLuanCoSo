using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace NLCS.Areas.Admin.ViewModels
{
    public class TeacherVM
    {
        
        [Required(ErrorMessage ="*")]
        [RegularExpression(@"^[A-Z]{2}\d{3}$", ErrorMessage = "ID phải có định dạng: 2 chữ cái hoa và 3 chữ số.")]
        public string Teacher_Id { get; set; }

        //Ten Giang vien
        [Required(ErrorMessage ="*")]
        public string Teacher_Name { get; set; }

        //Email
        [Required(ErrorMessage = "*")]
        [EmailAddress]
        public string Teacher_Email { get; set; }

        [Required(ErrorMessage = "*")]
        [MaxLength(20)]
        [MinLength(5)]
        public string Teacher_Username { get; set; }

        [Required(ErrorMessage = "*")]
        [MaxLength(8)]
        [MinLength(8, ErrorMessage = "ít nhất 8 ký tự!")]
        public string Teacher_Password { get; set; }

        //Hoc ham-Hoc vi
        [Required(ErrorMessage = "*")]
        public string Teacher_Degree { get; set; }

        //Anh Dai Dien
        [Required(ErrorMessage = "*")]
        public IFormFile Teacher_Image { get; set; }

        public string Teacher_Image_string { get; set; }
        //SDT
        [Required(ErrorMessage = "*")]
        public string Teacher_PhoneNumber { get; set; }

        public bool IsAdmin { get; set; } = false; // Mặc định là false

        public bool isActive { get; set; } = false; // Mặc định là false
        public string Teacher_FacultyID { get; set; }

        public List<SelectListItem> Degrees { get; set; }//Chỉ để hiện danh sách thôi
    }
}
