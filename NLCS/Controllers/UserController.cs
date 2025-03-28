using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using NLCS.Data;
using NLCS.Helper;
using NLCS.Models.Entities;
using NLCS.Repositories;
using NLCS.ViewModels;

namespace NLCS.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly AccountRepository accountRepository;
        private readonly IWebHostEnvironment env;
        public UserController(ApplicationDbContext db, AccountRepository accountRepository, IWebHostEnvironment env)
        {
            this.db = db;
            this.accountRepository = accountRepository;
            this.env = env;
        }
        public IActionResult Index()
        {
            return View();
        }
        #region DangKy
        [HttpGet]
        public IActionResult RegisterUser() 
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterUser(RegisterUserVM model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.DangKyModelError = "Thông tin không chính xác!";
                return View(model);
            }
            else
            {
                var user =  db.Students.SingleOrDefault(s => s.Student_UserName == model.Student_Username);
                if (user != null) {
                    ViewBag.AccountExisted = "Tài khoản đã tồn tại!";
                    return View(model);
                }
                else {
                    var result = new Student()
                    {
                        Student_Name = model.Student_Name,
                        Student_Email = model.Student_Email,
                        Student_DoB=model.Student_DoB,
                        Student_Address=model.Student_Address,
                        Student_UserName = model.Student_Username,
                        Student_Password=model.Student_Password,
                        Student_PhoneNumber=model.Student_PhoneNumber,
                        Gender = model.Student_Gender
                    };
                    await db.Students.AddAsync(result);
                    await db.SaveChangesAsync();
                    return RedirectToAction("LogInUser", "Account");
                }
            }
        }
        #endregion

        #region Profile User
        public async Task<IActionResult> Profile()
        {
            Student student = accountRepository.getCurrentStudent(User);
            if (student == null) {
                return RedirectToAction("LogInUser", "Account");
            }
            if (student.Student_Image == null) {
                student.Student_Image = "~/Assets/images/images.png";
            }

            return View(student);
        }
        #endregion

        #region Edit User
        [HttpGet]
        public async Task<IActionResult> EditUser(int id)
        {
            var student =  await db.Students.FirstOrDefaultAsync(s => s.Student_Id == id);

            if (student.Student_Image == null)
            {
                student.Student_Image = "~/Assets/images/images.png";
            }
            var studentVM = new StudentVM
            {
                Student_Name = student.Student_Name,
                Student_Password = student.Student_Password,
                Student_PhoneNumber = student.Student_PhoneNumber,
                Student_Address = student.Student_Address,
                Student_DoB = student.Student_DoB,
                Student_Email = student.Student_Email,
                Url_Image = student.Student_Image,
                Student_UserName = student.Student_UserName,
                Gender = student.Gender,
            };
            return View(studentVM); 
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(int id, StudentVM studentVM)
        {
            if (!ModelState.IsValid) 
            {
                return View(studentVM);
            }
            else
            {
                var student = await db.Students.FirstOrDefaultAsync(s => s.Student_Id == id);
                if (student == null)
                {
                    ViewBag.DoNotFound = "Không tìm thấy tài khoản!";
                    return View();
                }

                if (studentVM.Student_Image != null) 
                {
                    var imgPath = await ImageHelper.SaveImageAsync(studentVM.Student_Image, env);
                    student.Student_Image = imgPath;
                }

                student.Student_Address = studentVM.Student_Address;
                student.Student_DoB = (DateOnly)studentVM.Student_DoB;
                student.Student_PhoneNumber = studentVM.Student_PhoneNumber;
                student.Student_Name = studentVM.Student_Name;
                await db.SaveChangesAsync();
                return RedirectToAction("Profile","User", new {id = student.Student_Id});
            }
            return View(studentVM);
        }

        //ChangePassword
        [HttpGet]
        public async Task<IActionResult> ChangePassword(int id)
        {
            var user = await db.Students.FirstOrDefaultAsync(c => c.Student_Id==id);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(int id, ChangePasswordVM modelVM)
        {
            var user = await db.Students.FirstOrDefaultAsync(c => c.Student_Id == id);
            if(user.Student_Password != modelVM.OldPassword)
            {
                ViewBag.PasswordIsIncorrect = "Sai mật khẩu";
                return View();
            }
            else
            {
                user.Student_Password = modelVM.NewPassword;
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Profile", "User", new { id = user.Student_Id });
        }
        #endregion
    }
}
