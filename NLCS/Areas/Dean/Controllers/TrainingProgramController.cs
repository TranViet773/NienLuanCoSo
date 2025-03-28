using Microsoft.AspNetCore.Mvc;
using NLCS.Data;
using NLCS.Areas.Dean.ViewModels;
using Microsoft.EntityFrameworkCore;
using NLCS.Models.Entities;
using NLCS.Areas.Dean.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using NLCS.Areas.Admin.ViewModels;

namespace NLCS.Areas.Dean.Controllers
{
    [Area("Dean")]
    public class TrainingProgramController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly DeanRepository deanRepository;

        public TrainingProgramController(ApplicationDbContext db, DeanRepository deanRepository)
        {
            this.db = db;
            this.deanRepository = deanRepository;
        }

        [Authorize(Roles = "Dean")]
        public IActionResult getAllTP()// get all by id_khoa
        {
            string id_khoa = deanRepository.getCurrentTeacher(User).Teacher_FacultyID;
            var result = db.TrainingPrograms.Where(r => r.TP_FacultyId == id_khoa).ToList();
            return View(result);
        }

        #region Add Training Program
        [Authorize(Roles = "Dean")]
        [HttpGet]
        public async Task<IActionResult> AddTP()
        {
            TrainingProgramVM model = new TrainingProgramVM();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddTP(TrainingProgramVM modelVM)
        {
            modelVM.TP_FacultyId = "999";
            if (!ModelState.IsValid)
            {
                ViewBag.InValidTP = "Vui lòng điền đầy đủ thông tin!";
            }
            else
            {
                var tp = await db.TrainingPrograms.FirstOrDefaultAsync(tp => tp.TP_Id == modelVM.TP_Id || tp.TP_Name == modelVM.TP_Name);
                if (tp != null)
                {
                    ViewBag.ExistedTP = "Mã Chương trình đào tạo hoặc Tên bị trùng!";
                    return View(modelVM);
                }
                else
                {
                    string id_khoa = deanRepository.getCurrentTeacher(User).Teacher_FacultyID;
                    try
                    {
                        var trainingProgram = new TrainingProgram();
                        trainingProgram.TP_Id = modelVM.TP_Id;
                        trainingProgram.TP_Name = modelVM.TP_Name;
                        trainingProgram.TP_Target = modelVM.TP_Target;
                        trainingProgram.TP_Desciption = modelVM.TP_Desciption;
                        trainingProgram.TP_TrainingForm = modelVM.TP_TrainingForm;
                        trainingProgram.TP_Leader = modelVM.TP_Leader;
                        trainingProgram.TP_Title = modelVM.TP_Title;
                        trainingProgram.TP_FacultyId = id_khoa;
                        trainingProgram.TP_Student = modelVM.TP_Student;
                        await db.TrainingPrograms.AddAsync(trainingProgram);
                        await db.SaveChangesAsync();
                        return RedirectToAction("getAllTP", "TrainingProgram");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        return View();
                    }
                }
            }
            return View();
        }
        #endregion

        #region Details TrainingProgram
        public async Task<IActionResult> DetailTrainingProgram(string id)
        {
            TrainingProgram trainingProgram = await db.TrainingPrograms
                .Include(t => t.Faculty)
                .FirstOrDefaultAsync(t => t.TP_Id == id);
            if (trainingProgram == null)
            {
                ViewBag.DoNotFound = "Không tìm thấy Chương trình đào tạo!";
                return View();
            }
            else
            {
                return View(trainingProgram);
            }
            return View();
        }
        #endregion

        #region Edit Training Program
        // Cập nhattaj lại kểu dữ liệu của Training time
        [HttpGet]
        public async Task<IActionResult> EditTrainingProgram(string id)
        {
            var trainingProgram = await db.TrainingPrograms
                .Include(t => t.Faculty)
                .FirstOrDefaultAsync(t => t.TP_Id == id);
            var tpVM = new TrainingProgramVM();
            tpVM.TP_Id = id;
            tpVM.TP_Name = trainingProgram.TP_Name;
            tpVM.TP_Desciption = trainingProgram.TP_Desciption;
            tpVM.TP_Leader = trainingProgram.TP_Leader;
            tpVM.TP_TrainingForm = trainingProgram.TP_TrainingForm;
            tpVM.TP_Target = trainingProgram.TP_Target;
            tpVM.TP_Title = trainingProgram.TP_Title;
            tpVM.TP_Student = trainingProgram.TP_Student;
            return View(tpVM);
        }

        [HttpPost,ActionName("EditTrainingProgram")]
        public async Task<IActionResult> EditTP(string id, TrainingProgramVM modelVM)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TPinValid = "Vui lòng điền đầy đủ thông tin!";
                return View(modelVM);
            }
            else
            {
                var trainingProgram = await db.TrainingPrograms.FirstOrDefaultAsync(t => t.TP_Id == id);
                if (trainingProgram == null)
                {
                    ViewBag.TPWasNotFound = "Không tìm thấy Chương trình đào tạo!";
                    return View(modelVM);
                }
                else
                {
                    trainingProgram.TP_Name = modelVM.TP_Name;
                    trainingProgram.TP_Desciption = modelVM.TP_Desciption;
                    trainingProgram.TP_Leader = modelVM.TP_Leader;
                    trainingProgram.TP_TrainingForm = modelVM.TP_TrainingForm;
                    trainingProgram.TP_Target = modelVM.TP_Target;
                    trainingProgram.TP_Title = modelVM.TP_Title;
                    trainingProgram.TP_Student = modelVM.TP_Student;
                    db.SaveChanges();
                    return RedirectToAction("DetailTrainingProgram", "TrainingProgram", new { id = id });
                }
            }
            
        }
        #endregion

        #region Delete Training Program
        public async Task<IActionResult> DeleteTrainingProgram(string id)
        {
            var trainingProgram =  await db.TrainingPrograms.FirstOrDefaultAsync(t => t.TP_Id == id);
            if (trainingProgram == null) {
                ViewBag.DoNotFound = "Không tìm thấy Chương trình đào tạo!";
                return View();
            }
            var tpVM = new TrainingProgramVM();
            tpVM.TP_Id = id;
            tpVM.TP_Name = trainingProgram.TP_Name;
            tpVM.TP_Desciption = trainingProgram.TP_Desciption;
            tpVM.TP_Leader = trainingProgram.TP_Leader;
            tpVM.TP_TrainingForm = trainingProgram.TP_TrainingForm;
            tpVM.TP_Target = trainingProgram.TP_Target;
            tpVM.TP_Title = trainingProgram.TP_Title;
            tpVM.TP_Student = trainingProgram.TP_Student;
            tpVM.TP_FacultyId = trainingProgram.TP_FacultyId;
            return View(tpVM);
        }

        [HttpPost,ActionName("DeleteTrainingProgram")]
        public async Task<IActionResult> Delete(string id)
        {
            var trainingProgram = await db.TrainingPrograms.FirstOrDefaultAsync(t => t.TP_Id == id);
            db.TrainingPrograms.Remove(trainingProgram);
            db.SaveChanges();   
            return RedirectToAction("getAllTP");
        }
        #endregion
    }
}
