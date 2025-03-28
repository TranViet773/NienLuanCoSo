using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NLCS.Models.Entities
{
    [Table("Manager")]
    public class Manager
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Manager_Id { get; set; }

        [Required]
        [StringLength(255)]
        [Column(TypeName ="nvarchar")]
        public string Manager_Name { get; set; }

        [Required]
        public bool Manager_Sex { get; set; }

        [Required]
        public DateTime Manager_BornDate { get; set; }

        [Required]
        [StringLength(30)]
        public string Manager_UserName { get; set; }

        [Required]
        [StringLength(8)]
        public string Manager_Password { get; set; }

        [Required]
        [StringLength(50)]
        public string Manager_Email { get; set; }

        [Required]
        [MaxLength(10)]
        public string Manager_PhoneNumber { get; set; }

        [Required]
        [Column(TypeName="nvarchar")]
        [MaxLength(1024)]
        public string Manager_Information { get; set; }


        public ICollection<Faculty> Faculties { get; set; }
    }
}
