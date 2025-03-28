using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NLCS.Models.Entities
{
    public class DangKy
    {
        public int StudentID { get; set; }

        public Student Student { get; set; }

        [StringLength(10)]
        public string SubjectID { get; set; }
        public Subject Subject { get; set; }

        [StringLength(10)]
        public string Year { get; set; }
        public int Semester { get; set; }
        public DateOnly DateOfRegistration { get; set; }
        public DateOnly? DateOfCompletion { get; set; }
        //Many-One with TrangThaiDangKy
        public int DangKy_StatusId { get; set; }
        [ForeignKey("DangKy_StatusId")]
        public TrangThaiDangKy TrangThaiDangKy { get; set; }
    }
}
