using NLCS.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace NLCS.Areas.Admin.ViewModels
{
    public class RegisterVM
    {
        

        [Required(ErrorMessage = "Please enter your name!")]
        public string Manager_Name { get; set; }

        [Required(ErrorMessage = "Please choose your gender!")]
        public bool Manager_Sex { get; set; }

        [Required(ErrorMessage = "Please choose your born date!")]
        public DateTime Manager_BornDate { get; set; }


        [Required(ErrorMessage = "Please enter your username!")]
        public string Manager_UserName { get; set; }

        [Required(ErrorMessage = "Please enter your passsword!")]
        public string Manager_Password { get; set; }

        [Required(ErrorMessage = "Please enter your Email!")]
        public string Manager_Email { get; set; }


        public string Manager_PhoneNumber { get; set; }


        public string Manager_Information { get; set; }


        public ICollection<Faculty>? Faculties { get; set; }
    }
}
