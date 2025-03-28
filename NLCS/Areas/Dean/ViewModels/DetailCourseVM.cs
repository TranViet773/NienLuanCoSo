using Microsoft.AspNetCore.Mvc.Rendering;
using NLCS.Models.Entities;

namespace NLCS.Areas.Dean.ViewModels
{
    public class DetailCourseVM
    {
        
        public string Subject_Id { get; set; }

        //Ten HP
        public string Subject_Name { get; set; }

        //Hinh anh
        public IFormFile Subject_Image { get; set; }

        public string Url_Image { get; set; } = "/";

        //Thoi gian đào tạo
        public int Subject_Time { get; set; }

        //So luong
        public int Subject_Quantity { get; set; }
        //MoTa Noi Dung
        
        public string Subject_Description { get; set; }
        public string Subject_Content { get; set; }
        //Dia Diem to chuc
        
        public string Subject_Place { get; set; }
        //Doi tuong
        public string Subject_DoiTuong { get; set; }
        public List<string> TeacherIds { get; set; }
        public List<Teacher> Teachers { get; set; }
        public string Year { get; set; } 
        public int Semester { get; set; }

       

        }
    }
