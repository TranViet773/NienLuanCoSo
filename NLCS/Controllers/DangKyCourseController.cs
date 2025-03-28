using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLCS.Data;
using NLCS.Models.Entities;
using NLCS.Repositories;
using NLCS.ViewModels;

namespace NLCS.Controllers
{
    public class DangKyCourseController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly AccountRepository accountRepository;
        public DangKyCourseController(ApplicationDbContext db, AccountRepository accountRepository)
        {
            this.db = db;
            this.accountRepository = accountRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> getAll(int? id_trangthai)
        {
            int id_student = accountRepository.getCurrentStudent(User).Student_Id;
            IQueryable<DangKy> query = db.DangKys.Where(dk => dk.StudentID == id_student);
            ViewBag.ActiveTab = id_trangthai;

            if (id_trangthai.HasValue)
            {
                query = query.Where(dk => dk.DangKy_StatusId == id_trangthai.Value);
            }

            // Chuyển đổi kết quả thành danh sách CourseDangKyVM
            var courseRegistrations = await query
                .Select(dk => new CourseDangKyVM
                {
                    Subject_Id = dk.Subject.Subject_Id,
                    Subject_Name = dk.Subject.Subject_Name,
                    Year = dk.Year,
                    Student_Id = id_student,
                    Semester = dk.Semester,
                    Status_Dangky = dk.DangKy_StatusId,
                    DateOfRegistration = dk.DateOfRegistration,
                    DateOfCompletion = dk.DateOfCompletion ?? new DateOnly(2000, 1, 1)
                })
                .ToListAsync();

            return View(courseRegistrations);
        }



        [HttpPost]
        public async Task<IActionResult> DangKy(string id_course, string year, int semester)
        {
            var subject = await db.Courses.SingleOrDefaultAsync(s => s.Subject_Id == id_course);
            if (subject == null)
            {
                TempData["Error"] = "Khóa học không tồn tại!";
                return RedirectToAction("getAll", "Course");
            }

            var soluongdangky = await db.DangKys.CountAsync(t => t.SubjectID == id_course);
            if ((subject.Subject_Quantity - soluongdangky) <= 0)
            {
                TempData["FullSlot"] = "Khóa học hiện đã hết lượt đăng ký!";
                return RedirectToAction("getCourseById", "Course", new { id = id_course });
            }

            int id_student = accountRepository.getCurrentStudent(User).Student_Id;
            var registed = await db.DangKys.AnyAsync(t => t.StudentID == id_student && t.SubjectID == id_course);
            if (registed)
            {
                TempData["Registed"] = "Bạn đã đăng ký khóa học này!";
                return RedirectToAction("getCourseById", "Course", new { id = id_course });
            }

            // Kiểm tra và trừ số lượng môn học an toàn
            if (subject.Subject_Quantity > 0)
            {
                subject.Subject_Quantity -= 1;

                DangKy dangKy = new DangKy
                {
                    SubjectID = id_course,
                    StudentID = id_student,
                    Year = year,
                    Semester = semester,
                    DateOfRegistration = DateOnly.FromDateTime(DateTime.Now),
                    DangKy_StatusId = 1
                };

                await db.DangKys.AddAsync(dangKy);
                await db.SaveChangesAsync();
            }
            else
            {
                TempData["FullSlot"] = "Khóa học hiện đã hết lượt đăng ký!";
                return RedirectToAction("getCourseById", "Course", new { id = id_course });
            }

            return RedirectToAction("getAll", "DangKyCourse");
        }


        // tải bằng
        [HttpGet]
        public IActionResult DownloadCertificate(int studentId, string courseId)
        {
            // Xác định đường dẫn của file PDF đã tạo
            var filePath = Path.Combine(Path.GetTempPath(), $"Certificate_{studentId}_{courseId}.pdf");

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("Certificate not found. Please generate it first.");
            }

            // Trả về file PDF để người dùng tải
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/pdf", $"Certificate_{studentId}_{courseId}.pdf");
        }

    }
}
