using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NLCS.ViewModels
{
    public class RegisterUserVM
    {
        [Required(ErrorMessage = "*")]
        [StringLength(100)]
        [DisplayName("Fullname")]
        public string Student_Name { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(100)]
        [DisplayName("Email")]
        public string Student_Email { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Gender")]
        public bool Student_Gender { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Birthday")]
        public DateOnly Student_DoB { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(50)]
        [DisplayName("Username")]
        public string Student_Username { get; set; }

        [Required, StringLength(16)]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public string Student_Password { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(10)]
        [DisplayName("Phone Number")]
        public string Student_PhoneNumber { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(500)]
        [DisplayName("Address")]
        public string Student_Address { get; set; }
    }
}
