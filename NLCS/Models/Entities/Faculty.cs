using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NLCS.Models.Entities
{
    [Table("Faculty")]
    public class Faculty
    {
        [Key]//KHxxx
        [Required]
        [MaxLength(10)]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Faculty_Id { get; set; }
        //Ten Khoa
        [Required]
        [StringLength(50)]
        public string Faculty_Name { get; set; }

        //Mo ta
        [Required,MaxLength(1024)]
        public string Faculty_Description { get; set; }
        //Image
        [MaxLength(1024)]
        public string Faculty_Image { get; set; }
        //Khóa ngoại tham chiếu tới Manager
        public int Faculty_ManagerId {  get; set; }
        [ForeignKey("Faculty_ManagerId")]
        public Manager Manager { get; set; }
        
        //Khóa ngoại cho Major
        public virtual ICollection<TrainingProgram> TrainingPrograms { get; set; }

        //-> Teacher
        public virtual ICollection<Teacher> Teachers { get; set; }

    }
}
