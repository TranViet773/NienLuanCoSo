using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLCS.Areas.Admin.Repositories;
using NLCS.Areas.Admin.ViewModels;
using NLCS.Data;
using NLCS.Helper;
using NLCS.Models.Entities;

namespace NLCS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FacultyController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly ManagerRepository _managerRepository;
        
        private readonly IWebHostEnvironment _env;
        public FacultyController(ApplicationDbContext db, ManagerRepository managerRepository, IWebHostEnvironment env)
        {
            this.db = db;
            _managerRepository = managerRepository;
            this._env = env;
        }
        public IActionResult Index()
        {
            var result = db.Faculties.ToList();
            return View(result);
        }

        #region Add Faculty
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AddFaculty() 
        { 
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddFaculty(FacultyVM model)
        {
            if(!ModelState.IsValid)
            {
                ViewBag.AddFacultyError = "Thông tin không hợp lệ!";
                return View(model);
            }
            else
            {

                var faculty = await db.Faculties.FirstOrDefaultAsync(f => f.Faculty_Id == model.ID);
                if (faculty != null) 
                {
                    ViewBag.ExistIDFaculty = "Mã khoa đã tồn tại, Vui lòng đăng kí mã khoa khác!";
                    return View(model);
                }
                else
                {
                    
                    if (model.Image != null)
                    {
                        string imagePath = await ImageHelper.SaveImageAsync(model.Image, _env);
                       
                        // Lưu đường dẫn vào database hoặc xử lý tiếp theo
                        var managerId = _managerRepository.getCurrentManager(User).Manager_Id;
                        var result = new Faculty()
                        {
                            Faculty_Id = model.ID,
                            Faculty_Name = model.Name,
                            Faculty_Description = model.Description,
                            Faculty_Image = imagePath,
                            Faculty_ManagerId = managerId,
                        };
                        await db.Faculties.AddAsync(result);
                        await db.SaveChangesAsync();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View(model);
                    }
                    
                }
            }
        }
        #endregion

        #region Detail Faculty
        public IActionResult DetailFaculty(string id)
        {
            var result = db.Faculties
                .Include(f => f.TrainingPrograms)
                .Include(f => f.Teachers)
                .FirstOrDefault(f => f.Faculty_Id == id);
            return View(result);
        }
        #endregion

        #region Edit Faculty
        [HttpGet]
        public IActionResult EditFaculty(string id)
        {
            var result = db.Faculties.FirstOrDefault(f => f.Faculty_Id == id);
            var facultyVM = new FacultyVM()
            {
                ID = result.Faculty_Id,
                Name = result.Faculty_Name,
                Description = result.Faculty_Description,
                ImageUrl = result.Faculty_Image,
            };
            return View(facultyVM);
        }

        [HttpPost]
        public async Task<IActionResult> EditFaculty(string id, FacultyVM model)
        {
            var faculty = await db.Faculties.FirstOrDefaultAsync(f => f.Faculty_Id == id);
            if (faculty != null) 
            {
                try
                {
                    if (!ModelState.IsValid) 
                    {
                        ViewBag.ModelStateInvalid = "Sai thông tin!";
                        return View();
                    }
                    else
                    {
                        if(model.Image != null)
                        {
                            string imagePath = await ImageHelper.SaveImageAsync(model.Image, _env);
                            // Lưu đường dẫn vào database hoặc xử lý tiếp theo
                            var managerId = _managerRepository.getCurrentManager(User).Manager_Id;
                            faculty.Faculty_Name = model.Name;
                            faculty.Faculty_Image = imagePath;
                            faculty.Faculty_Description = model.Description;
                           faculty.Faculty_ManagerId = managerId;
                            await db.SaveChangesAsync();
                            return RedirectToAction("DetailFaculty","Faculty", new {id = faculty.Faculty_Id});
                        }
                    }
                }
                catch (Exception ex) 
                {
                    ViewBag.ex = ex.Message;
                    Console.WriteLine(ex.ToString());
                }
            }
            return View(model);
        }
        #endregion

        #region Delete Faculty
        [HttpGet]
        public async Task<IActionResult> DeleteFaculty(string id)
        {
            var faculty = await db.Faculties.FirstOrDefaultAsync(f => f.Faculty_Id == id);
            
            return View(faculty);
        }

        [HttpPost, ActionName("DeleteFaculty")]
        public async Task<IActionResult> Delete(string id)
        {
            
            
            var faculty = await db.Faculties.FirstOrDefaultAsync(f => f.Faculty_Id == id);
            if(faculty != null)
            {
                try
                {
                    if (faculty.Faculty_Image != null)
                    {
                        await ImageHelper.DeleteImageAsync(faculty.Faculty_Image, _env);
                    }
                    db.Faculties.Remove(faculty);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index","Faculty");
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }           
            }
            return View();
        }
        #endregion
    }
}
