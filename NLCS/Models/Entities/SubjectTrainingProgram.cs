using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NLCS.Models.Entities
{
    public class SubjectTrainingProgram
    {
        public string TPID { get; set; }
        [ForeignKey("TPID")]
        public TrainingProgram trainingProgram { get; set; }


        [StringLength(10)]
        public string SubjectID { get; set; }
        [ForeignKey("SubjectID")]
        public Subject subject { get; set; }
    }
}
