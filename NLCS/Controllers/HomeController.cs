using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.EntityFrameworkCore;
using NLCS.Areas.Admin.Controllers;
using NLCS.Data;
using NLCS.Models;
using NLCS.Models.Entities;
using NLCS.ViewModels;
using System.Diagnostics;

namespace NLCS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            db = applicationDbContext;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Test()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult About() 
        {
            return View();
        }

        public async Task<IActionResult> getFaculty(string id)
        {
            var faculty = await db.Faculties
                .Include(f => f.Teachers)
                .Include(f => f.TrainingPrograms)
                .FirstOrDefaultAsync( f => f.Faculty_Id == id);
            return View(faculty);
        }

        public async Task<IActionResult> DetailTeacher(string id)
        {
            var teacher = await db.Teachers
                .Include(t => t.Facultys)                  // Bao gồm thông tin khoa (Faculty)
                .Include(t => t.GiangDays)                // Bao gồm danh sách giảng dạy (GiangDays)
                    .ThenInclude(gd => gd.subject)
                .FirstOrDefaultAsync(t => t.Teacher_Id == id);
            var teacherVM = new TeacherVM {
                Teacher_Id = teacher.Teacher_Id,
                Teacher_Degree = teacher.Teacher_Degree,
                Teacher_Email = teacher.Teacher_Email,
                Teacher_Image = teacher.Teacher_Image,
                Teacher_Name = teacher.Teacher_Name,
                Teacher_PhoneNumber = teacher.Teacher_PhoneNumber,
                IsAdmin = teacher.IsAdmin,
                Faculty_Name = teacher.Facultys.Faculty_Name,
                Subjects = teacher.GiangDays.Select(gd => gd.subject).ToList(),
            };
            return View(teacherVM);
        }

        [HttpGet]
        public async Task<IActionResult> search(string query)
        {
            return View();
        }

        [HttpPost, ActionName("search")]
        public async Task<IActionResult> handleSearch(string query)
        {
            var byId = await db.Courses
                           .Where(s => s.Subject_Id.Contains(query))
                           .ToListAsync();

            if(!byId.Any())
            {
                var byName = await db.Courses
                          .Where(s => s.Subject_Name.Contains(query))
                          .ToListAsync();
                if (!byName.Any())
                {
                    ViewBag.DoNotFound = "Không tìm thấy khóa học!";
                    return View();
                }
                else
                {
                    return View(byName);
                }
            }
            else
            {
                return View(byId);
            }
        }
    }
}
