using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NLCS.Data;
using NLCS.Models.Entities;
using System.Security.Claims;
using NLCS.Areas.Admin.ViewModels;
using Microsoft.EntityFrameworkCore;
using NLCS.Areas.Admin.Repositories;

namespace NLCS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ManagerController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly ManagerRepository _managerRepository;  // Chỉnh sửa

        public ManagerController(ApplicationDbContext db, ManagerRepository managerRepository)
        {
            this.db = db;
            _managerRepository = managerRepository ?? throw new ArgumentNullException(nameof(managerRepository));
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var list = await db.Managers.ToListAsync();
            return View(list);
       }

        #region DangKy

        //DangKy
        [HttpGet]
        public IActionResult Register()
        {
            var model = new RegisterVM();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(vm);
            }
            else
            {
                try
                {
                    var manager = await db.Managers.FirstOrDefaultAsync(p => p.Manager_UserName == vm.Manager_UserName);
                    if (manager != null)
                    {
                        ViewBag.ExistedAccount = "Account is existed!";
                        return View();
                    }
                    else
                    {

                        //gọi hàm để tạo mã và gửi email
                        var code = await _managerRepository.GenerateAndSendVerificationCodeAsync(vm.Manager_Email);
                        //Lưu thông tin(code, vm vào Session)
                        //Do sử dụng popup nên không thể dùng TempData - chỉ truyền qua 
                        HttpContext.Session.SetString("RegisterVM", JsonConvert.SerializeObject(vm));
                        HttpContext.Session.SetString("VerifyCode", code);
                    }

                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return View(vm);
                }
            }
        }

        // So khớp mã xác nhận
        public async Task<IActionResult> VerifyCode(string code1, string code2, string code3, string code4, string code5, string code6)
        {
            var verifyCode = $"{code1}{code2}{code3}{code4}{code5}{code6}";
            //Lấy thông tin từ session
            var code = HttpContext.Session.GetString("VerifyCode");
            var vmJson = HttpContext.Session.GetString("RegisterVM");
            if (code == null || vmJson == null)
            {
                return Json(new { success = false, message = "Session hết hạn!" });
            }
            else
            {
                var vm = JsonConvert.DeserializeObject<RegisterVM>(vmJson);

                //so sánh mã xác nhận
                if (verifyCode != code)
                {
                    return Json(new { success = false, message = "Mã xác nhận không hợp lệ!" });
                }
                //Thêm người dùng vào db
                var result = new Manager();
                result.Manager_Name = vm.Manager_Name;
                result.Manager_Sex = vm.Manager_Sex;
                result.Manager_Email = vm.Manager_Email;
                result.Manager_BornDate = vm.Manager_BornDate;
                result.Manager_UserName = vm.Manager_UserName;
                result.Manager_Password = vm.Manager_Password;
                result.Manager_PhoneNumber = vm.Manager_PhoneNumber;
                result.Manager_Information = vm.Manager_Information;


                await db.Managers.AddAsync(result);
                await db.SaveChangesAsync();

                // Xóa thông tin khỏi session
                HttpContext.Session.Remove("RegisterVM");
                HttpContext.Session.Remove("VerifyCode");

                return Json(new { success = true });
            }
        }
        #endregion

        //Tạo một khoa mới

    }
}
