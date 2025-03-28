using NLCS.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace NLCS.ViewModels
{
    public class TeacherVM
    {
        public string Teacher_Id { get; set; }
        public string Teacher_Name { get; set; }
        public string Teacher_Email { get; set; }
        public string Teacher_Username { get; set; }
        public string Teacher_Password { get; set; }
        public string Teacher_Degree { get; set; }
        [Required]
        public string Teacher_Image { get; set; }
        public string Teacher_PhoneNumber { get; set; }
        public string Faculty_Name { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
        public bool IsAdmin { get; set; }
        public bool isActive { get; set; }

        public bool isDisplay { get; set; } 
    }
}
