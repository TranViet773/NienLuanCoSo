using System.ComponentModel.DataAnnotations;

namespace NLCS.ViewModels
{
    public class ChangePasswordVM
    {
        [Required(ErrorMessage = "*")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "*")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "*")]
        [Compare("NewPassword", ErrorMessage = "Do not match!")]
        public string ConfirmPassword { get; set; }
    }
}
