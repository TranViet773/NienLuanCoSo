


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NLCS.Models.Entities
{
    [Table("Student")]
    public class Student
    {
        [Key]
       
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Student_Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Student_Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Student_Email { get; set; }

        [Required]
        public DateOnly Student_DoB { get; set; }

        public string? Student_Image { get; set; }

        [Required]
        public bool Gender { get; set; }

        [Required]
        [StringLength(50)]
        public string Student_UserName { get; set; }

        [Required,StringLength(16)]
        public string Student_Password { get; set; }

        [Required]
        [StringLength(10)]
        public string Student_PhoneNumber { get; set; }

        [Required]
        [StringLength(500)]
        public string Student_Address { get; set; }

        //Relationship
        //->Subjects
        public virtual ICollection<DangKy> DangKy { get; set; }

    }
}
