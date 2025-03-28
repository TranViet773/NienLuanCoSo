using System.ComponentModel.DataAnnotations;

namespace NLCS.ViewModels
{
    public class LogInUserVM
    {
        [Required(ErrorMessage = "Please enter your username!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your username!")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please enter your password!")]
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
