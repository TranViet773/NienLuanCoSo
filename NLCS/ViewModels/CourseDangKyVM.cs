using NLCS.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NLCS.ViewModels
{
    public class CourseDangKyVM
    {
        public string Subject_Id { get; set; }
        public string Subject_Name { get; set; }
        public int Student_Id { get; set; }
        public string Year { get; set; }
        public int Semester { get; set; }
        public int Status_Dangky { get; set; }
        public DateOnly DateOfRegistration { get; set; }
        public DateOnly DateOfCompletion { get; set; }
    }
}
