using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NLCS.Models.Entities
{
    [Table("Teacher")]
    public class Teacher
    {
        [Key]//CBxxx
        [Required]
        [StringLength(10)]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Teacher_Id { get; set; }

        //Ten Giang vien
        [Required]
        [MaxLength(100)]
        public string Teacher_Name { get; set; }

        //Email
        [Required]
        [MaxLength (100)]
        public string Teacher_Email { get; set;}
        [Required]
        public string Teacher_Username { get; set;}
        [Required]
        public string Teacher_Password { get; set;}

        //Hoc ham-Hoc vi
        [Required]
        [MaxLength(100)]
        public string Teacher_Degree { get; set; }

        //Anh Dai Dien
        [MaxLength(1024)]
        public string Teacher_Image { get; set; }

        //SDT
        [Required]
        [MaxLength(10)]
        public string Teacher_PhoneNumber { get; set; }

        public bool IsAdmin { get; set; } = false; // Mặc định là false

        public bool isActive { get; set; } = true; // Mặc định là false

        public bool isDisplay { get; set; } = false; // ghim

        //Cac Tham Chieu
        //-> Subject
        public virtual ICollection<Subject> Subjects { get; set; }

        //->Faculty (Khoa)
        public string Teacher_FacultyID { get; set; }
        [ForeignKey("Teacher_FacultyID")]
        public Faculty Facultys { get; set; }

        //-> GiangDay
        public virtual ICollection<GiangDay> GiangDays { get; set; }
    }

}
