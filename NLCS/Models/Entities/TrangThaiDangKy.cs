using System.ComponentModel.DataAnnotations;

namespace NLCS.Models.Entities
{
    public class TrangThaiDangKy
    {
        [Key]
        [Required]
        public int Status_Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status_Name { get; set; }

        //One-Many with DangKy
        public ICollection<DangKy> DangKy { get; set; }
    }
}
