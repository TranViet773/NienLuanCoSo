using NLCS.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NLCS.Areas.Dean.ViewModels
{
    public class TrainingProgramVM
    {
        [Required(ErrorMessage="*")]
        public string TP_Id { get; set; }

        //ten CTDT
        [Required(ErrorMessage="*")]
        public string TP_Name { get; set; }

        ////thoi gian dao tao
        //[Required(ErrorMessage="*")]
        //public DateTime TP_TrainingTime { get; set; }

        //Doi tuong hoc vien
        [Required(ErrorMessage="*")]        
        public string TP_Student { get; set; }

        //Can Bo Nghien Cuu
        [Required(ErrorMessage="*")]
        public string TP_Leader { get; set; }


        //Danh hieu
        [Required(ErrorMessage="*")]
        public string TP_Title { get; set; }

        //Hinh thuc dao tao
        [Required(ErrorMessage="*")]
        [StringLength(100)]
        public string TP_TrainingForm { get; set; }


        //Muc tieu
        [Required(ErrorMessage="*")]
        public string TP_Target { get; set; }

        //Mo ta chung
        [Required(ErrorMessage="*")]
        public string TP_Desciption { get; set; }

        [Required(ErrorMessage = "*")]
        public string TP_FacultyId { get; set; } = "999";


        [NotMapped]
        public List<SelectListItem> HinhThucDaoTaos { get; set; }//Chỉ để hiện danh sách thôi
        public List<SelectListItem> DoiTuongDaoTaos { get; set; }=new List<SelectListItem>();//Chỉ để hiện danh sách thôi
        public List<SelectListItem> MucTieugDaoTaos { get; set; }= new List<SelectListItem>();//Chỉ để hiện danh sách thôi
        public TrainingProgramVM()
        {
            HinhThucDaoTaos = new List<SelectListItem>
            {
            new SelectListItem { Value = "Chính qui", Text = "Chính qui" },
            new SelectListItem { Value = "Từ xa", Text = "Từ xa" },
            new SelectListItem { Value = "Vừa học vừa làm", Text = "Vừa học vừa làm" }
            };
            DoiTuongDaoTaos = new List<SelectListItem>
            {
                new SelectListItem { Value = "Cao Đẳng trở lên", Text = "Cao Đẳng trở lên" },
                new SelectListItem { Value = "Đại Học", Text = "Đại Học" }
            };
        }

    }
}
