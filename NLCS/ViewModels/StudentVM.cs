using System.ComponentModel.DataAnnotations;

namespace NLCS.ViewModels
{
    public class StudentVM
    {
        public string Student_Name { get; set; }

        public string? Student_Email { get; set; }

        public DateOnly? Student_DoB { get; set; }

        public IFormFile? Student_Image { get; set; } 

        public string? Url_Image { get; set; } = "/";

        public bool? Gender { get; set; }

        public string? Student_UserName { get; set; }

        public string? Student_Password { get; set; }

        public string? Student_PhoneNumber { get; set; }

        public string? Student_Address { get; set; }
    }
}
