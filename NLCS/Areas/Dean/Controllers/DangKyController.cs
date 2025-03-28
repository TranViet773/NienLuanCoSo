using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLCS.Areas.Dean.Repositories;
using NLCS.Areas.Dean.ViewModels;
using NLCS.Data;
using NLCS.Areas.Dean.Services;
using System.Net.NetworkInformation;

namespace NLCS.Areas.Dean.Controllers
{
    [Area("Dean")]
    public class DangKyController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly DeanRepository deanRepository;
        private readonly IPdfService pdfService;
        public DangKyController(ApplicationDbContext db, DeanRepository deanRepository, IPdfService pdfService)
        {
            this.db = db;
            this.deanRepository = deanRepository;
            this.pdfService = pdfService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> getAllRegistration(int? id_trangthai, string year, int? semester)
        {
            var id_faculty = deanRepository.getCurrentTeacher(User).Teacher_FacultyID;
            ViewBag.ActiveTab = id_trangthai;
            ViewBag.SelectedSemester = semester;
            ViewBag.SelectedYear = year;
            // Lấy tất cả các khóa học thuộc khoa này
            var coursesInFaculty = db.Courses
                .Where(course => db.SubjectTrainingPrograms
                    .Any(stp => stp.SubjectID == course.Subject_Id &&
                        db.TrainingPrograms.Any(tp => tp.TP_Id == stp.TPID && tp.TP_FacultyId == id_faculty)))
                .Select(course => course.Subject_Id)
                .ToList();

            // Lấy thông tin đăng ký dựa trên các khóa học thuộc khoa, trạng thái và điều kiện year/semester
            var registrations = db.DangKys
                .Where(dk => coursesInFaculty.Contains(dk.SubjectID) &&
                            (!id_trangthai.HasValue || dk.DangKy_StatusId == id_trangthai) &&
                            (string.IsNullOrEmpty(year) || dk.Year == year) &&
                            (!semester.HasValue || dk.Semester == semester)
                        )   // Chỉ lọc theo semester nếu semester có giá trị
                .Include(dk => dk.Student)
                .Select(dk => new DangKyVM
                {
                    Subject_Id = dk.Subject.Subject_Id,
                    Student_Id = dk.Student.Student_Id,
                    Subject_Name = dk.Subject.Subject_Name,
                    Student_Username = dk.Student.Student_UserName,
                    Student_Name = dk.Student.Student_Name,
                    Student_Email = dk.Student.Student_Email,
                    Semester = dk.Semester,
                    Year = dk.Year,
                    DateOfRegistration = dk.DateOfRegistration,
                    DangKy_Status = dk.DangKy_StatusId
                })
                .ToList();
            return View(registrations);
        }

        //Duyệt
        public async Task<IActionResult> ComfirmRegistation(int id_student, string id_subject, int id_status, string year, int semester)
        {
            var dangky = await db.DangKys.FirstOrDefaultAsync(dk => dk.SubjectID == id_subject && dk.StudentID == id_student);
            dangky.DangKy_StatusId = id_status+1;
            await db.SaveChangesAsync();
            return RedirectToAction("getAllRegistration", new {id_trangthai = id_status, year = year, semester = semester});
        }
        //Xác nhận hủy
        public async Task<IActionResult> DeleteRegistation(int id_student, string id_subject, int id_status, string year, int semester)
        {
            var dangky = await db.DangKys.FirstOrDefaultAsync(dk => dk.SubjectID == id_subject && dk.StudentID == id_student);
            db.DangKys.Remove(dangky);
            await db.SaveChangesAsync();
            return RedirectToAction("getAllRegistration", new { id_trangthai = id_status, year = year, semester = semester });
        }

        //Cấp bằng
        public async Task<IActionResult> GenerateCertificate(int studentId, string courseId, int id_status)
        {
           //Chuyển trạng thia và cập nhật ngày đăng ký
            var dangky = await db.DangKys.FirstOrDefaultAsync(dk => dk.SubjectID == courseId && dk.StudentID == studentId);
            dangky.DangKy_StatusId = id_status + 1;
            dangky.DateOfCompletion = DateOnly.FromDateTime(DateTime.Now);
            await db.SaveChangesAsync();

            //cấp bằng
            var student = await db.Students.FirstOrDefaultAsync(s => s.Student_Id == studentId);
            var course = await db.Courses.FirstOrDefaultAsync(c => c.Subject_Id == courseId);
            var teacher = deanRepository.getCurrentTeacher(User);

            if (student == null || course == null)
            {
                return NotFound();
            }

            // Khởi tạo dữ liệu cho template
            var certificateData = new CertificateViewModel
            {
                StudentName = student.Student_Name,
                CourseName = course.Subject_Name,
                CompletionDate = DateTime.Now,
                RegistationDate = dangky.DateOfRegistration.ToDateTime(TimeOnly.Parse("10:00 PM")),
                ChucDanh = teacher.Teacher_Degree,
                NguoiCapBang = teacher.Teacher_Name
                
            };
       

            var htmlContent = pdfService.GenerateHtmlCertificate(certificateData);
            var pdfFile = pdfService.GenerateCertificatePdf(htmlContent);

            // Lưu PDF vào thư mục tạm
            var filePath = Path.Combine(Path.GetTempPath(), $"Certificate_{studentId}_{courseId}.pdf");
            System.IO.File.WriteAllBytes(filePath, pdfFile);

            // Chuyển hướng người dùng tới trang có nút tải file hoặc thông báo đã tạo thành công
            TempData["TaoBangThanhCong"] = "Đã tạo bằng thành công!";
            return RedirectToAction("getAllRegistration");
        }


    }
}
