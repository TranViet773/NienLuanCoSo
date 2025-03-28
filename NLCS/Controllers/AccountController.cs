using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using NLCS.Areas.Admin.ViewModels;
using NLCS.ViewModels;
using System.Security.Claims;
using NLCS.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;
using NLCS.Areas.Admin.Repositories;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using NLCS.Repositories;
using NLCS.Models.Entities;
namespace NLCS.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountRepository _accountRepositpory;
        private readonly ApplicationDbContext db;
        public AccountController(ApplicationDbContext db, AccountRepository accountRepository)
        {
            this.db = db;
            _accountRepositpory = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult getImageOfUser() 
        {
            var username = User.Identity.Name;
            var image = _accountRepositpory.getImageByUsername(username);
            ViewBag.UserName = username;
            ViewBag.TeacherImage = image;
            return View();
        }

        #region DangNhap
        [HttpGet]
        public IActionResult LogInUser(string? ReturnUrl)
        {
            ViewBag.ReturnURL = ReturnUrl;
            return View();
        }

        [HttpPost, ActionName("LogInUser")]
        public async Task<IActionResult> LogInUser(LogInUserVM modelVM, string? ReturnUrl)
        {
            if (ReturnUrl == null) {
                ReturnUrl = "/";
            }
            var role = modelVM.Role;
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(modelVM);
            }
            else
            {
                if (role == "1") // Student
                {
                    var student = await db.Students.FirstOrDefaultAsync(m => m.Student_UserName == modelVM.UserName && m.Student_Password == modelVM.Password && modelVM.Email == m.Student_Email);
                    if (student == null)
                    {
                        return View(modelVM);
                    }
                    else
                    {
                        // gọi hàm để tạo mã và gửi email
                        var code = await _accountRepositpory.GenerateAndSendVerificationCodeAsync(modelVM.Email);
                        //Lưu thông tin(code, vm vào Session)
                        //Do sử dụng popup nên không thể dùng TempData - chỉ truyền qua 
                        HttpContext.Session.SetString("LogInUserVM", JsonConvert.SerializeObject(modelVM));
                        HttpContext.Session.SetString("VerifyCode", code);
                        HttpContext.Session.SetString("ReturnUrl", ReturnUrl);
                        return Json(new { success = true });
                    }
                }
                else if (role == "2") //Dean
                {
                    
                    var teacher = await db.Teachers.FirstOrDefaultAsync(m => m.Teacher_Username == modelVM.UserName && m.Teacher_Password == modelVM.Password && modelVM.Email == m.Teacher_Email);
                    if (teacher == null)
                    {
                        return View(modelVM);
                    }
                    else
                        if (teacher.IsAdmin)
                        {
                            // gọi hàm để tạo mã và gửi email
                            var code = await _accountRepositpory.GenerateAndSendVerificationCodeAsync(modelVM.Email);
                            //Lưu thông tin(code, vm vào Session)
                            //Do sử dụng popup nên không thể dùng TempData - chỉ truyền qua 
                            HttpContext.Session.SetString("LogInUserVM", JsonConvert.SerializeObject(modelVM));
                            HttpContext.Session.SetString("VerifyCode", code);
                            HttpContext.Session.SetString("ReturnUrl", ReturnUrl);
                            return Json(new { success = true });
                        }
                        else
                        {
                            string message = "Tài khoản không quyền quản trị";
                            return Json(new { success = false, mess = message });
                        }
                }
                else if (role == "3") // Admin
                {
                    var manager = await db.Managers.FirstOrDefaultAsync(m => m.Manager_UserName == modelVM.UserName && m.Manager_Password == modelVM.Password && modelVM.Email == m.Manager_Email);
                    if (manager == null)
                    {
                        return View(modelVM);
                    }
                    else
                    {
                        // gọi hàm để tạo mã và gửi email
                        var code = await _accountRepositpory.GenerateAndSendVerificationCodeAsync(modelVM.Email);
                        //Lưu thông tin(code, vm vào Session)
                        //Do sử dụng popup nên không thể dùng TempData - chỉ truyền qua 
                        HttpContext.Session.SetString("LogInUserVM", JsonConvert.SerializeObject(modelVM));
                        HttpContext.Session.SetString("VerifyCode", code);
                        HttpContext.Session.SetString("ReturnUrl", ReturnUrl);
                        return Json(new { success = true });
                    }
                }
            }
            return Json(new { success = false });
        }

        public async Task<IActionResult> VerifyCode(string code1, string code2, string code3, string code4, string code5, string code6)
        {
            var verifyCode = $"{code1}{code2}{code3}{code4}{code5}{code6}";
            //Lấy thông tin từ session
            var code = HttpContext.Session.GetString("VerifyCode");
            var vmJson = HttpContext.Session.GetString("LogInUserVM");
            var returnUrl = HttpContext.Session.GetString("ReturnUrl");
            if (code == null || vmJson == null)
            {
                return Json(new { success = false, message = "Mã xác thực hết hiệu lực, vui lòng thử lại!" });
            }
            else
            {
                var vm = JsonConvert.DeserializeObject<LogInUserVM>(vmJson);

                //so sánh mã xác nhận
                if (verifyCode != code)
                {
                    return Json(new { success = false, message = "Mã xác nhận không hợp lệ!" });
                }
                //Thực hiện lưu người dùng
                var roleClaim = "";
                if (vm.Role == "1")
                    roleClaim = "Student";
                if (vm.Role == "2")
                    roleClaim = "Dean";
                if (vm.Role == "3")
                    roleClaim = "Admin";
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, vm.Email),
                        new Claim(ClaimTypes.Name, vm.UserName),
                        //Claim động
                        new Claim(ClaimTypes.Role, roleClaim)
                    };
                var claimsIdentity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);
                var claimPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(claimPrincipal);

                // Xóa thông tin khỏi session
                HttpContext.Session.Remove("LogInUserVM");
                HttpContext.Session.Remove("VerifyCode");
                HttpContext.Session.Remove("ReturnUrl");
                //Đăng nhập thành công
                return Json(new { success = true, role = vm.Role, url = returnUrl});
            }
        }

        #endregion

        //Log Out
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Index","Home");
        }
    }
}
