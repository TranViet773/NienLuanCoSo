using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using NLCS.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NLCS.Areas.Dean.ViewModels
{
    public class CourseVM
    {
        [Required(ErrorMessage = "*")]
        public string Subject_Id { get; set; }

        //Ten HP
        [Required(ErrorMessage = "*")]
        public string Subject_Name { get; set; }


        //Hinh anh
        [Required(ErrorMessage = "*")]
        public IFormFile Subject_Image { get; set; }

        public string Url_Image { get; set; } = "/";

        //Thoi gian đào tạo
        [Required(ErrorMessage = "*")]
        public int Subject_Time { get; set; }
        //So luong
        [Required(ErrorMessage = "*")]
        public int Subject_Quantity { get; set; }
        //MoTa Noi Dung
        [Required(ErrorMessage = "*")]
        public string Subject_Description { get; set; }
        //Noi Dung Chi Tiet
        public string Subject_Content { get; set; }
        //Dia Diem to chuc
        [Required(ErrorMessage = "*")]
        public string Subject_Place { get; set; }
        //Doi tuong
        [Required(ErrorMessage = "*")]
        public string Subject_DoiTuong { get; set; }
        public List<SelectListItem> DanhSachDoiTuong {get; set;} = new List<SelectListItem>();
        public List<SelectListItem> Teachers { get; set; } = new List<SelectListItem>();
        public List<string> SelectedTeacherIds { get; set; } // danh sách các ID giáo viên đã chọn

        public List<SelectListItem> TrainingPrograms { get; set; } = new List<SelectListItem>();
        public List<string> SelectedTrainingProgramIds { get; set; } // danh sách các ID CTDT đã chọn


        public List<SelectListItem> Years { get; set; } // Danh sách năm học

        [Required(ErrorMessage = "*")]
        public string Year { get; set; } // Giá trị năm học đã chọn
        public List<SelectListItem> Semesters { get; set; } = new List<SelectListItem>(); // Danh sách Học kì
        public int Semester { get; set; }

        public CourseVM()
        {
            Years = new List<SelectListItem>
            {
            new SelectListItem { Value = "20242025", Text = "2024 - 2025" },
            new SelectListItem { Value = "20252026", Text = "2025 - 2026" },
            new SelectListItem { Value = "20262027", Text = "2026 -2027" }
            };


            DanhSachDoiTuong = new List<SelectListItem>
            {
                new SelectListItem{Value = "Nhân viên kỹ thuật, Nhân viên vận hành, Sinh viên, Cán bộ nghiên cứu", Text = "Tất cả"},
                new SelectListItem{Value = "Nhân viên kỹ thuật", Text = "Nhân viên kỹ thuật"},
                new SelectListItem{Value = "Nhân viên vận hành", Text = "Nhân viên vận hành"},
                new SelectListItem{Value = "Sinh viên", Text = "Sinh viên"},
                new SelectListItem{Value = "Cán bộ nghiên cứu", Text = "Cán bộ nghiên cứu"}
            };

            Semesters = new List<SelectListItem>
            {
                new SelectListItem{Value = "1", Text = "1"},
                new SelectListItem{Value = "2", Text = "2"},
                new SelectListItem{Value = "3", Text = "3"},


            };

        }
    }
}
