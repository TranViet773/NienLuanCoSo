using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NLCS.Areas.Admin.ViewModels;
using NLCS.Areas.Dean.Repositories;
using NLCS.Areas.Dean.ViewModels;
using NLCS.Data;
using NLCS.Helper;
using NLCS.Models.Entities;
using System.Text;

namespace NLCS.Areas.Dean.Controllers
{
    [Area("Dean")]
    public class HomeController : Controller
    {
        private readonly DeanRepository deanRepository;
        private readonly ApplicationDbContext db;
        private readonly IWebHostEnvironment env;
        public HomeController(ApplicationDbContext db, DeanRepository deanRepository, IWebHostEnvironment env)
        {
            this.db = db;
            this.deanRepository = deanRepository;
            this.env = env; 
        }
        public async Task<IActionResult> Index()
        {
            string facultyId = deanRepository.getCurrentTeacher(User).Teacher_FacultyID;
            var quantityOfTP = db.Faculties
                .Where(f => f.Faculty_Id == facultyId)
                .SelectMany(f => f.TrainingPrograms)
                .Count();

            var quantityOfCourses = await db.Faculties
                .Where(f => f.Faculty_Id == facultyId)
                .SelectMany(f => f.TrainingPrograms)
                .SelectMany(tp => tp.SubjectTrainingPrograms)
                .Select(stp => stp.subject) // Lấy các khóa học từ SubjectTrainingPrograms
                .Distinct() // Loại bỏ các khóa học trùng lặp
                .CountAsync();
            var quantityOfRegister = await db.Faculties
                .Where(f => f.Faculty_Id == facultyId)
                .SelectMany(f => f.TrainingPrograms)
                .SelectMany(tp => tp.SubjectTrainingPrograms)
                .Select(stp => stp.subject)
                .Distinct()
                .SelectMany(s => s.DangKy)
                .CountAsync();

            var courseRegistrations = await db.Faculties
                .Where(f => f.Faculty_Id == facultyId)
                .SelectMany(f => f.TrainingPrograms)
                .SelectMany(tp => tp.SubjectTrainingPrograms)
                .GroupBy(stp => stp.subject.Subject_Name) 
                .Select(g => new
                {
                    CourseName = g.Key, 
                    RegistrationCount = g.SelectMany(stp => stp.subject.DangKy).Count()
                })
                .ToListAsync();
            var courseNames = courseRegistrations.Select(c => c.CourseName).ToList();
            ViewBag.CourseNames = JsonConvert.SerializeObject(courseNames);
            var registrationCount = courseRegistrations.Select(c => c.RegistrationCount).ToList();
            ViewBag.RegistrationCount = JsonConvert.SerializeObject(registrationCount);

            ViewBag.QuantityOdRegister = quantityOfRegister;
            ViewBag.QuantityOfCourse = quantityOfCourses;
            ViewBag.QuantityOfTP = quantityOfTP;
            return View();
        }

        public async Task<IActionResult> Profile()
        {
            Teacher teacher = deanRepository.getCurrentTeacher(User);
            if (teacher == null)
            {
                return RedirectToAction("LogInUser", "Account");
            }
            var result = await db.Teachers
                .Include(t => t.Facultys)
                .FirstOrDefaultAsync(t => t.Teacher_Id == teacher.Teacher_Id);
            if (teacher.Teacher_Image == null)
            {
                teacher.Teacher_Image = "~/Assets/images/images.png";
            }
            return View(teacher);
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile(string id)
        {
            var teacher = await db.Teachers.FirstOrDefaultAsync(t => t.Teacher_Id == id);
            if(teacher.Teacher_Image == null)
            {
                teacher.Teacher_Image = "~/Assets/images/images.png";
            }
            var teacherVM = new EditTeacherVM
            {
                Teacher_Name = teacher.Teacher_Name,
                Teacher_Email = teacher.Teacher_Email,
                Teacher_Degree = teacher.Teacher_Degree,
                Teacher_PhoneNumber = teacher.Teacher_PhoneNumber,
                Url_Image = teacher.Teacher_Image
            };
            return View(teacherVM);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(string id, EditTeacherVM model)
        {
            var teacher = await db.Teachers.FirstOrDefaultAsync(t => t.Teacher_Id == id);
            if (teacher == null) 
            {
                ViewBag.DoNotFound = "Không tìm thấy thông tin!";
                return View();
            }
            if (model.Teacher_Image != null)
            {
                var imgPath = await ImageHelper.SaveImageAsync(model.Teacher_Image, env);
                teacher.Teacher_Image = imgPath;
            }

            teacher.Teacher_PhoneNumber = model.Teacher_PhoneNumber;
            teacher.Teacher_Name = model.Teacher_Name;
            await db.SaveChangesAsync();
            return RedirectToAction("Profile", "Home");
        }
    }
}
