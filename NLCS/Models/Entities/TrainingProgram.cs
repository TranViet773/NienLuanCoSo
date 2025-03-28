using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NLCS.Models.Entities
{
    public class TrainingProgram
    {
        [Key]//CTxxx
        [Required]
        [MaxLength(10)]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string TP_Id { get; set; }

        //ten CTDT
        [Required]
        [StringLength(255)]
        [Column(TypeName = "nvarchar")]
        public string TP_Name { get; set; }

        //Doi tuong hoc vien
        [Required]
        [StringLength(255)]
        public string TP_Student {  get; set; }

        //Can Bo Nghien Cuu
        [Required]
        [StringLength(255)]
        public string TP_Leader { get; set; }  


        //Danh hieu
        [Required]
        [StringLength(255)]
        public string TP_Title { get; set; }

        //Hinh thuc dao tao
        [Required]
        [StringLength(100)]
        public string TP_TrainingForm {  get; set; }


        //Muc tieu
        [Required]
        [StringLength(1024)]
        public string TP_Target { get; set; }

        //Mo ta chung
        [Required]
        [StringLength(1024)]
        public string TP_Desciption { get; set; }


        //-> Major (KHOA)
        public string TP_FacultyId { get; set; }
        [ForeignKey("TP_FacultyId")]
        public Faculty Faculty { get; set; }

        // -> Subjects (HocPhan)
        // Mối quan hệ với Subject
        public virtual ICollection<Subject> Subjects { get; set; }

        //-> SubjectTrainingProgram
        public virtual ICollection<SubjectTrainingProgram> SubjectTrainingPrograms { get; set; } = new List<SubjectTrainingProgram>();

    }
}
