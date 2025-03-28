using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NLCS.Models.Entities
{
    public class GiangDay
    {

        public string TeacherID { get; set; }
        [ForeignKey("TeacherID")]
        public Teacher teacher { get; set; }

        
        [StringLength(10)]
        public string SubjectID { get; set; }
        [ForeignKey("SubjectID")]
        public Subject subject { get; set; }

        [StringLength(10)]
        public string Year { get; set; }
        public int Semester { get; set; }

    }
}
