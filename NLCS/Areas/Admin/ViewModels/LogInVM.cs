using System.ComponentModel.DataAnnotations;

namespace NLCS.Areas.Admin.ViewModels
{
    public class LogInVM
    {
        [Required(ErrorMessage = "Please enter your username!")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please enter your password!")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please confirm password!")]
        [Compare("Password", ErrorMessage = "Do not match password!")]
        public string Confirm_Password { get; set; }
    }
}
