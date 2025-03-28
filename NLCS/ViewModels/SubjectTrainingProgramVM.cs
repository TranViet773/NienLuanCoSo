using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NLCS.ViewModels
{
    public class SubjectTrainingProgramVM
    {
        public string SubjectName { get; set; }
        public List<string> TrainingPrograms { get; set; }
        public string Subject_Id { get; set; }
        public string Subject_Name { get; set; }
        public string Subject_Image { get; set; }
        public int Subject_Time { get; set; }
        public int Subject_Quantity { get; set; }
        public string Subject_Description { get; set; }
        public string Subject_Content { get; set; }
        public string Subject_Place { get; set; }
        public string Subject_DoiTuong { get; set; }
    }
}
