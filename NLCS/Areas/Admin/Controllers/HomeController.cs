using Microsoft.AspNetCore.Mvc;
using NLCS.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace NLCS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db;

        public HomeController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<IActionResult> Index() // Count the Student, Faculty and Teacher
        {
            // Số lượng sinh viên, khoa, giảng viên
            var stu_quantity = db.Students.Count();
            var faculties_quantity = db.Faculties.Count();
            var teacher_quantity = db.Teachers.Count();

            // Truy vấn số lượng giảng viên theo khoa
            var facultiesWithCounts = db.Faculties
                .Select(f => new
                {
                    f.Faculty_Name, // Tên khoa
                    LecturerCount = db.Teachers.Count(t => t.Teacher_FacultyID == f.Faculty_Id) // Số giảng viên của khoa
                })
                .ToList();

            // Truy vấn số lượng đăng ký môn học theo khoa
            var facultySubjectData = db.Faculties
                .Select(f => new {
                    Faculty_Name = f.Faculty_Name,
                    StudentCount = f.TrainingPrograms
                        .SelectMany(tp => tp.SubjectTrainingPrograms) // Liên kết từ CTDT sang SubjectTrainingProgram
                        .SelectMany(stp => stp.subject.DangKy)         // Liên kết từ SubjectTrainingProgram đến các đăng ký môn học
                        .Count()                                      // Đếm tổng số đăng ký
                })
                .ToList();

            // Gửi dữ liệu vào View
            ViewBag.NumOfFaculties = faculties_quantity;
            ViewBag.NumOfStudents = stu_quantity;
            ViewBag.NumOfTeachers = teacher_quantity;
            ViewBag.FacultyData = facultiesWithCounts;
            ViewBag.FacultySubjectData = facultySubjectData;

            return View();
        }
    }
}
