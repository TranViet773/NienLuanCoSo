using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLCS.Data;
using NLCS.Models.Entities;
using NLCS.ViewModels;

namespace NLCS.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext db;
        public CourseController(ApplicationDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetCourseByTrainingProgram(string id_trainingprogram, string name_tp)
        {
            var courses = await db.Courses
            .Include(c => c.SubjectTrainingPrograms) // Bao gồm thông tin chương trình đào tạo
            .Where(c => c.SubjectTrainingPrograms.Any(tp => tp.TPID == id_trainingprogram)) // Kiểm tra nếu có chương trình đào tạo nào trùng với TPID
            .ToListAsync();
            Console.WriteLine(courses); // Kiểm tra danh sách môn học trong console

            if (!courses.Any())
            {
                Console.WriteLine("No subjects found for the given Training Program ID.");
            }

            ViewBag.TrainingProgram = name_tp;
            return View(courses);
        }

        public async Task<IActionResult> getCourseById(string id, string? nametp)
        {   var soluongdangky = await db.DangKys.CountAsync(t => t.SubjectID == id);     
            var subjectWithTrainingPrograms = await db.Courses
                .Where(s => s.Subject_Id == id)
                .Select(s => new SubjectTrainingProgramVM
                {
                    Subject_Name = s.Subject_Name,
                    Subject_Id = s.Subject_Id,
                    Subject_DoiTuong = s.Subject_DoiTuong,
                    Subject_Image = s.Subject_Image,
                    Subject_Place = s.Subject_Place,
                    Subject_Quantity = s.Subject_Quantity, //Slot conf lai - dang ky
                    Subject_Description = s.Subject_Description,
                    Subject_Time = s.Subject_Time,
                    TrainingPrograms = s.SubjectTrainingPrograms
                        .Select(stp => stp.trainingProgram.TP_Name)
                        .ToList()
                })
                .FirstOrDefaultAsync(); // Sử dụng phiên bản async

            if (subjectWithTrainingPrograms == null)
            {
                return NotFound(); // Trả về NotFound nếu không tìm thấy course
            }
            ViewBag.TrainingProgram = nametp;
            return View(subjectWithTrainingPrograms); // Trả về view với dữ liệu
        }
    }
}
