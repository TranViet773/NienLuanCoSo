using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NLCS.Models.Entities
{
    [Table("Subject")]
    public class Subject
    {
        [Key]//HPxxx
        [Required]
        [StringLength(10)]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Subject_Id { get; set; }

        //Ten HP
        [Required]
        [MaxLength(255)]
        [Column(TypeName = "nvarchar")]
        public string Subject_Name { get; set; }


        //Hinh anh
        [Required, MaxLength(1024)]
        public string Subject_Image {  get; set; }

        
        //Thoi gian đào tạo
        [Required]
        public int Subject_Time { get; set; }
        //So luong
        [Required]
        public int Subject_Quantity { get; set; }

        //MoTa Noi Dung
        [Required]
        public string Subject_Description { get; set; }


        //Mo ta chi tiet
        public string Subject_Content { get; set; }

        //Dia Diem to chuc
        [Required, MaxLength(100)]
        public string Subject_Place { get; set; }
        //Doi tuong
        [Required]
        public string Subject_DoiTuong { get; set; }
        //Cac Tham Chieu
        //-> TrainingProgram (CTDT)public string TP_FacultyId { get; set; }
        //public string Subject_TPId { get; set; }
        //[ForeignKey("Subject_TPId")]
        //public TrainingProgram TrainingProgram { get; set; }
        public ICollection<TrainingProgram> TrainingPrograms { get; set; }

        //-> Teacher
        public virtual ICollection<Teacher> Teachers { get; set; }

        //->Student
        public virtual ICollection<DangKy> DangKy { get; set; }

        //-> GiangDay
        public virtual ICollection<GiangDay> GiangDays { get; set; }

        //-> SubjectTrainingProgram
        public virtual ICollection<SubjectTrainingProgram> SubjectTrainingPrograms { get; set; } = new List<SubjectTrainingProgram>();

    }
}
