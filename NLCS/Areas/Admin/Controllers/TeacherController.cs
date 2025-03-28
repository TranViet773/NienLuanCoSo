using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NLCS.Areas.Admin.Repositories;
using NLCS.Areas.Admin.ViewModels;
using NLCS.Areas.Dean.Repositories;
using NLCS.Data;
using NLCS.Helper;
using NLCS.Models.Entities;
using NuGet.DependencyResolver;
using NuGet.Protocol.Plugins;
using System.Diagnostics.Contracts;

namespace NLCS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeacherController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly ManagerRepository managerRepository;
        private readonly IWebHostEnvironment env;
        public TeacherController(ApplicationDbContext _db, ManagerRepository _managerRepository, IWebHostEnvironment _env) 
        {
            db = _db;   
            managerRepository = _managerRepository;
            env = _env;
        }

        public IActionResult Index()
        {
           var result = db.Teachers
                .Include(t => t.Facultys)
                .ToList();
            return View(result);
        }

        #region Add Teacher
        private async Task<List<SelectListItem>> GetFacultiesAsync()
        {
            return await db.Faculties.Select(f => new SelectListItem
            {
                Value = f.Faculty_Id,
                Text = f.Faculty_Name
            }).ToListAsync();
        }
        [HttpGet]
        public async Task<IActionResult> AddTeacher()
        {
            var model = new TeacherVM();
            // Khởi tạo danh sách Faculties và truyền vào ViewBag
            ViewBag.Faculties = await GetFacultiesAsync();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddTeacher(TeacherVM model)
        {
            var teacher = await db.Teachers.FirstOrDefaultAsync(t => t.Teacher_Id == model.Teacher_Id);
            if (teacher != null)
            {
                ViewBag.ExistedTeacher = "Mã giáo viên đã tồn tại!";
                ViewBag.Faculties = await GetFacultiesAsync();
                return View(model);
            }

            // Thực hiện thêm Teacher
            if (model.Teacher_Image != null)
            {
                string imagePath = await ImageHelper.SaveImageAsync(model.Teacher_Image, env);
                var result = new Teacher()
                {
                    Teacher_Id = model.Teacher_Id,
                    Teacher_Name = model.Teacher_Name,
                    Teacher_Password = model.Teacher_Password,
                    Teacher_Username = model.Teacher_Username,
                    Teacher_Image = imagePath,
                    Teacher_Degree = model.Teacher_Degree,
                    Teacher_Email = model.Teacher_Email,
                    Teacher_PhoneNumber = model.Teacher_PhoneNumber,
                    IsAdmin = model.IsAdmin,
                    Teacher_FacultyID = model.Teacher_FacultyID,
                };
                await db.Teachers.AddAsync(result);
                await db.SaveChangesAsync();
                return RedirectToAction("Search", "Teacher");
            }
            // Nếu không có hình ảnh, trả về model
            ViewBag.Faculties = await GetFacultiesAsync();
            return View(model);
        }
        #endregion

        #region Detail Teacher
        public IActionResult DetailTeacher(string id)
        {
            var result = db.Teachers
                .Include(t => t.Facultys)
                .FirstOrDefault(t => t.Teacher_Id == id);
            return View(result);
        }

        #endregion

        #region Edit Teacher
        [HttpGet]
        public async Task<IActionResult> EditTeacher(string id)
        {
            var result = db.Teachers
                .Include(t => t.Facultys)
                .FirstOrDefault(t => t.Teacher_Id == id);
           
            var teacherVM = new TeacherVM
            {
                Teacher_Id = result.Teacher_Id,
                Teacher_Name = result.Teacher_Name,
                Teacher_Email = result.Teacher_Email,
                Teacher_PhoneNumber = result.Teacher_PhoneNumber,
                Teacher_Degree = result.Teacher_Degree,
                Teacher_Image_string = result.Teacher_Image,
                Teacher_Password = result.Teacher_Password,
                Teacher_Username = result.Teacher_Username, 
                Teacher_FacultyID = result.Teacher_FacultyID,
                isActive = result.isActive,
                IsAdmin = result.IsAdmin
                // Ánh xạ các thuộc tính khác nếu cần
            };
            teacherVM.Degrees = new List<SelectListItem>
            {
                new SelectListItem { Value = "Tiến sĩ", Text = "Tiến sĩ" },
                new SelectListItem { Value = "Thạc sĩ", Text = "Thạc sĩ" },
                new SelectListItem { Value = "Cử nhân", Text = "Cử nhân" }
            };
            ViewBag.Faculties = await GetFacultiesAsync();
            return View(teacherVM);
        }

        [HttpPost]
        public async Task<IActionResult> EditTeacher(string id, TeacherVM model)
        {

            var teacher = await db.Teachers.FirstOrDefaultAsync(t => t.Teacher_Id == id);
            if (teacher == null) {
                return View(teacher);
            }
            else
            {
                if (model.Teacher_Image != null)
                {
                    string imagePath = await ImageHelper.SaveImageAsync(model.Teacher_Image, env);
                    teacher.Teacher_Image = imagePath;
                }
                if (model.IsAdmin)
                {
                    var dean = await db.Teachers.FirstOrDefaultAsync(t => t.IsAdmin == true && t.Teacher_FacultyID == teacher.Teacher_FacultyID);
                    if (dean != null)
                    {
                        dean.IsAdmin = false;
                    }
                }
                    teacher.Teacher_Id = model.Teacher_Id;
                    teacher.Teacher_Name = model.Teacher_Name;
                    teacher.Teacher_Password = model.Teacher_Password;
                    teacher.Teacher_Username = model.Teacher_Username;
                    teacher.Teacher_Degree = model.Teacher_Degree;
                    teacher.Teacher_Email = model.Teacher_Email;
                    teacher.Teacher_PhoneNumber = model.Teacher_PhoneNumber;
                    teacher.IsAdmin = model.IsAdmin;
                    teacher.isActive = model.isActive;
                    teacher.Teacher_FacultyID = model.Teacher_FacultyID;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Search", "Teacher");
            }
            return View();
        }
        #endregion

        #region Delete Teacher
        [HttpGet]
        public IActionResult DeleteTeacher(String id)
        {
            var result = db.Teachers
                .Include(t => t.Facultys)
                .FirstOrDefault(t => t.Teacher_Id == id);
            return View(result);
        }
        [HttpPost, ActionName("DeleteTeacher")]
        public async Task<IActionResult> Delete(string id)
        {
            var teacher = await db.Teachers.FirstOrDefaultAsync(t => t.Teacher_Id == id);
            if(teacher == null)
            {
                ViewBag.DoNotFoundTeacher = "Không tìm thấy Giảng Viên cần xóa!";
                return View(teacher);
            }
            else
            {
                try
                {
                    if (teacher.Teacher_Image != null)
                    {
                        await ImageHelper.DeleteImageAsync(teacher.Teacher_Image, env);
                    }
                    db.Teachers.Remove(teacher);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index", "Teacher");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            return View();
        }

        #endregion

        #region Search
        private async Task<List<SelectListItem>> GetListFaculty()
        {
            return await db.Faculties
                .Select(f => new SelectListItem
                {
                    Value = f.Faculty_Id,
                    Text = f.Faculty_Name
                }).ToListAsync();
        }

        [HttpGet]
        public async Task<IActionResult> Search()
        {
            var teachers = db.Teachers
                .Include(c => c.Facultys)
                .ToList();
            //var courses = await db.Courses
            //.Include(c => c.SubjectTrainingPrograms) // Bao gồm thông tin chương trình đào tạo
            //.Where(c => c.SubjectTrainingPrograms.Any(tp => tp.TPID == "TP002")) // Kiểm tra nếu có chương trình đào tạo nào trùng với TPID
            //.ToListAsync();
            ViewBag.Faculties = await GetListFaculty();
            return View(teachers);
        }

        [HttpPost]
        public async Task<IActionResult> Search(string id_Faculty)
        {
            if(id_Faculty == null)//get all
            {
                var teachers = await db.Teachers // Bao gồm thông tin chương trình đào tạo
                    .Include(c => c.Facultys)// Kiểm tra nếu có chương trình đào tạo nào trùng với TPID
                    .ToListAsync();
                    ViewBag.Faculties = await GetListFaculty();
                return View(teachers);
            }
            else// lấy theo id khoa
            {
                var faculty = await db.Teachers // Bao gồm thông tin chương trình đào tạo
                   .Where(c => c.Teacher_FacultyID == id_Faculty)
                   .Include(c => c.Facultys)// Kiểm tra nếu có chương trình đào tạo nào trùng với TPID
                   .ToListAsync();
                ViewBag.Faculties = await GetListFaculty();
                ViewBag.FacultyIsSelected =  db.Faculties.FirstOrDefault(f => f.Faculty_Id == id_Faculty).Faculty_Name;
                return View(faculty);
            }
        }
        #endregion

        #region Search by ID,Name
        [HttpPost]
        public async Task<IActionResult> SearchByNameOrId(string search)
        {
            var teacher = await db.Teachers
                .Include(c => c.Facultys)
                .Where(t => t.Teacher_Id.Contains(search)).ToListAsync();
            if (!teacher.Any())
            {
                 teacher = await db.Teachers
                    .Include(c => c.Facultys)
                    .Where(t => t.Teacher_Name.Contains(search))
                    .ToListAsync();
            }
            if(!teacher.Any())
            {
                ViewBag.DoNotFound = "Không tìm thấy giảng viên!";
            }
            ViewBag.Faculties = await GetListFaculty();
            return View(teacher);
        }
        #endregion
    }

}
