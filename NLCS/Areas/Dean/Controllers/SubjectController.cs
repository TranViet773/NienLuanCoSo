using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NLCS.Areas.Dean.Repositories;
using NLCS.Areas.Dean.ViewModels;
using NLCS.Data;
using NLCS.Helper;
using NLCS.Models.Entities;
using NLCS.Repositories;

namespace NLCS.Areas.Dean.Controllers
{
    [Area("Dean")]
    public class SubjectController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IWebHostEnvironment env;
        private readonly DeanRepository deanRepository;
        public SubjectController(ApplicationDbContext db, IWebHostEnvironment env, DeanRepository deanRepository)
        {
            this.db = db;
            this.env = env;
            this.deanRepository = deanRepository;   
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult getAllCourses()
        {
            var course = db.Courses
                .Include(c => c.TrainingPrograms)
                .ToList();
            return View(course);
        }

        #region Add Course
        [HttpGet]
        public async Task<IActionResult> AddCourse()
        {
            var model = new CourseVM
            {
                Teachers = await db.Teachers.Select(t => new SelectListItem
                {
                    Value = t.Teacher_Id, // Giá trị của item
                    Text = t.Teacher_Name // Văn bản hiển thị
                }).ToListAsync(), // Sử dụng ToListAsync để làm việc với Entity Framework
                TrainingPrograms = await db.TrainingPrograms.Select(t => new SelectListItem
                {
                    Value = t.TP_Id, // Giá trị của item
                    Text = t.TP_Name // Văn bản hiển thị
                }).ToListAsync()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCourse(CourseVM modelVM)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CourseInvalid = "Vui lòng điền đầy đủ thông tin!";
                modelVM.Teachers = await db.Teachers.Select(t => new SelectListItem
                {
                    Value = t.Teacher_Id, // Giá trị của item
                    Text = t.Teacher_Name // Văn bản hiển thị
                }).ToListAsync(); // Sử dụng ToListAsync để làm việc với Entity Framework
                modelVM.TrainingPrograms = await db.TrainingPrograms.Select(t => new SelectListItem
                {
                    Value = t.TP_Id, // Giá trị của item
                    Text = t.TP_Name // Văn bản hiển thị
                }).ToListAsync();
                return View(modelVM);
            }
            else
            {
                var course = await db.Courses.FirstOrDefaultAsync(c => c.Subject_Id == modelVM.Subject_Id || c.Subject_Name == modelVM.Subject_Name);
                if (course != null)
                {
                    ViewBag.CourseWasExisted = "Tên hoặc mã đã bị trùng";
                    modelVM.Teachers = await db.Teachers.Select(t => new SelectListItem
                    {
                        Value = t.Teacher_Id, // Giá trị của item
                        Text = t.Teacher_Name // Văn bản hiển thị
                    }).ToListAsync(); // Sử dụng ToListAsync để làm việc với Entity Framework
                    modelVM.TrainingPrograms = await db.TrainingPrograms.Select(t => new SelectListItem
                    {
                        Value = t.TP_Id, // Giá trị của item
                        Text = t.TP_Name // Văn bản hiển thị
                    }).ToListAsync();
                    return View(modelVM);
                }
                else
                {
                    if (modelVM.Subject_Image != null)
                    {
                        string imgPath = await ImageHelper.SaveImageAsync(modelVM.Subject_Image, env);
                        var subject = new Subject
                        {
                            Subject_Id = modelVM.Subject_Id,
                            Subject_Name = modelVM.Subject_Name,
                            Subject_Description = modelVM.Subject_Description,
                            Subject_Content=modelVM.Subject_Content,
                            Subject_DoiTuong = modelVM.Subject_DoiTuong,
                            Subject_Image = imgPath,
                            Subject_Place = modelVM.Subject_Place,
                            Subject_Quantity = modelVM.Subject_Quantity,
                            Subject_Time = modelVM.Subject_Time,
                            GiangDays = new List<GiangDay>()
                        };
                        // Thêm quan hệ giáo viên
                        foreach (var teacherId in modelVM.SelectedTeacherIds)
                        {
                            subject.GiangDays.Add(new GiangDay
                            {
                                SubjectID = subject.Subject_Id,
                                TeacherID = teacherId.ToString(),
                                Year = modelVM.Year,
                                Semester = modelVM.Semester
                            });
                        }
                        // Thêm quan hệ CTDT
                        foreach (var tpId in modelVM.SelectedTrainingProgramIds)
                        {
                            subject.SubjectTrainingPrograms.Add(new SubjectTrainingProgram
                            {
                                SubjectID = subject.Subject_Id,
                                TPID = tpId.ToString(),
                            });
                        }

                        db.Courses.Add(subject);
                        await db.SaveChangesAsync();
                        return RedirectToAction("Search"); // Redirect đến danh sách môn học
                    }
                    else
                    {
                        ViewBag.ImageisNull = "Vui lòng chọn hình ảnh!";
                        return View(modelVM);
                    }
                }
            }

        }
        #endregion

        #region Details Course

        public async Task<List<Teacher>> GetListTeacher(string id_course)
        {
            // Lấy danh sách id_teacher cho khóa học cụ thể
            var teacherIds = await db.GiangDays
                .Where(g => g.SubjectID == id_course)
                .Select(g => g.TeacherID)
                .ToListAsync();

            // Lấy danh sách giảng viên dựa trên danh sách id_teacher
            var teachers = await db.Teachers
                .Where(t => teacherIds.Contains(t.Teacher_Id))
                .ToListAsync(); // Trả về danh sách đầy đủ các đối tượng Teacher

            return teachers;
        }

        public async Task<int?> getSemester(string id_course, string id_teacher)
        {
            var semester = await db.GiangDays
                .Where(g => g.SubjectID == id_course && g.TeacherID == id_teacher)
                .Select(g => g.Semester)
                .FirstOrDefaultAsync(); // Lấy giá trị đầu tiên hoặc giá trị mặc định (null)

            return semester;
        }

        public async Task<string> getYear(string id_course, string id_teacher)
        {
            var year = await db.GiangDays
                .Where(g => g.SubjectID == id_course && g.TeacherID == id_teacher)
                .Select(g => g.Year)
                .FirstOrDefaultAsync(); // Lấy giá trị đầu tiên hoặc giá trị mặc định (null)

            return year;
        }

        public async Task<IActionResult> DetailCourse(string id)
        {
            var model = await db.Courses
                .Include(c => c.GiangDays) // Bao gồm danh sách GiangDays
                .Include(g => g.Teachers) // Bao gồm thông tin giảng viên
                .FirstOrDefaultAsync(c => c.Subject_Id == id);
            if (model == null)
            {
                ViewBag.DoNotFound = "Không tìm thấy khóa học!";
                return View();
            }
            else
            {
                DetailCourseVM courseVM = new DetailCourseVM
                {
                    Subject_Id = model.Subject_Id,
                    Subject_Name = model.Subject_Name,
                    Url_Image = model.Subject_Image,
                    Subject_Description = model.Subject_Description,
                    Subject_Content = model.Subject_Content,
                    Subject_DoiTuong = model.Subject_DoiTuong,
                    Subject_Place = model.Subject_Place,
                    Subject_Quantity = model.Subject_Quantity,
                    Subject_Time = model.Subject_Time,
                    Teachers = await GetListTeacher(model.Subject_Id) // Lấy danh sách giảng viên dạy khóa học
                };
                return View(courseVM);
            }
            #endregion

        }

        #region Edit Course
        [HttpGet]
        public async Task<IActionResult> EditCourse(string id)
        {
            string faculty_id = deanRepository.getCurrentTeacher(User).Teacher_FacultyID;
            var course = await db.Courses.FirstOrDefaultAsync(c => c.Subject_Id == id);
            var model = new CourseVM
            {
                Subject_Id = course.Subject_Id,
                Subject_Name = course.Subject_Name,
                Url_Image = course.Subject_Image,
                Subject_Description = course.Subject_Description,
                Subject_DoiTuong = course.Subject_DoiTuong,
                Subject_Place = course.Subject_Place,
                Subject_Quantity = course.Subject_Quantity,
                Subject_Time = course.Subject_Time,

                Teachers = await db.Teachers.Select(t => new SelectListItem
                {
                    Value = t.Teacher_Id, // Giá trị của item
                    Text = t.Teacher_Name // Văn bản hiển thị
                }).ToListAsync(), // Sử dụng ToListAsync để làm việc với Entity Framework

                TrainingPrograms = await db.TrainingPrograms
                .Where(t => t.TP_FacultyId == faculty_id)
                .Select(t => new SelectListItem
                {
                    Value = t.TP_Id, // Giá trị của item
                    Text = t.TP_Name // Văn bản hiển thị
                }).ToListAsync()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditCourse(string id, CourseVM modelVM)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ModelIsInValid = "Vui lòng điền đầy đủ thông tin!";
                return View(modelVM);
            }

            var course = await db.Courses.Include(c => c.GiangDays).Include(c => c.SubjectTrainingPrograms)
                                           .SingleOrDefaultAsync(c => c.Subject_Id == id);
            if (course == null)
            {
                return NotFound(); // Xử lý khi khóa chính không tồn tại
            }

            if (modelVM.Subject_Image != null)
            {
                string imgPath = await ImageHelper.SaveImageAsync(modelVM.Subject_Image, env);
                course.Subject_Name = modelVM.Subject_Name;
                course.Subject_Description = modelVM.Subject_Description;
                course.Subject_Content = modelVM.Subject_Content;
                course.Subject_DoiTuong = modelVM.Subject_DoiTuong;
                course.Subject_Image = imgPath;
                course.Subject_Place = modelVM.Subject_Place;
                course.Subject_Quantity = modelVM.Subject_Quantity;
                course.Subject_Time = modelVM.Subject_Time;

                // Cập nhật quan hệ giáo viên
                course.GiangDays.Clear(); // Xóa các mục cũ
                foreach (var teacherId in modelVM.SelectedTeacherIds)
                {
                    course.GiangDays.Add(new GiangDay
                    {
                        SubjectID = course.Subject_Id,
                        TeacherID = teacherId.ToString(),
                        Year = modelVM.Year,
                        Semester = modelVM.Semester
                    });
                }

                // Cập nhật quan hệ CTDT
                course.SubjectTrainingPrograms.Clear(); // Xóa các mục cũ
                foreach (var tpId in modelVM.SelectedTrainingProgramIds)
                {
                    course.SubjectTrainingPrograms.Add(new SubjectTrainingProgram
                    {
                        SubjectID = course.Subject_Id,
                        TPID = tpId.ToString(),
                    });
                }

                await db.SaveChangesAsync();
                return RedirectToAction("Search"); // Redirect đến danh sách môn học
            }
            else
            {
                ViewBag.ImageisNull = "Vui lòng chọn hình ảnh!";
                return View(modelVM);
            }
        }

        #endregion

        #region Delete Course
        [HttpGet]
        public async Task<IActionResult> DeleteCourse(string id)
        {
            var result = await db.Courses.FirstOrDefaultAsync(c => c.Subject_Id == id);
            return View(result);
        }

        [HttpPost,ActionName("DeleteCourse")]
        public async Task<IActionResult> Delete(string id)
        {
            var course = await db.Courses.Include(c => c.GiangDays)
                                           .Include(c => c.SubjectTrainingPrograms)
                                           .SingleOrDefaultAsync(c => c.Subject_Id == id);

            if (course == null)
            {
                ViewBag.DoNotFound = "Không tìm thấy khóa học!";
                return View(); // Xử lý khi không tìm thấy khóa học
            }

            // Xóa các quan hệ trước khi xóa khóa học
            db.GiangDays.RemoveRange(course.GiangDays);
            db.SubjectTrainingPrograms.RemoveRange(course.SubjectTrainingPrograms);

            db.Courses.Remove(course);
            ViewBag.TrainingProgram = await GetTPsAsync();
            await db.SaveChangesAsync();
            return RedirectToAction("Search"); // Redirect đến danh sách môn học
        }

        #endregion

        #region Search
        private async Task<List<SelectListItem>> GetTPsAsync()
        {
            string faculty_id = deanRepository.getCurrentTeacher(User).Teacher_FacultyID;
            return await db.TrainingPrograms
                .Where(f => f.TP_FacultyId == faculty_id)
                .Select(f => new SelectListItem
            {
                Value = f.TP_Id,
                Text = f.TP_Name
            }).ToListAsync();
        }
        [HttpGet]
        public async Task<IActionResult> Search() // tương đương getAll List
        {
            var id_teacher = deanRepository.getCurrentTeacher(User).Teacher_Id;
            var courses = db.Courses
                .Include(c => c.TrainingPrograms)
                .Where(c => c.GiangDays.Any(gd => gd.TeacherID == id_teacher))
                .ToList();
            //var courses = await db.Courses
            //.Include(c => c.SubjectTrainingPrograms) // Bao gồm thông tin chương trình đào tạo
            //.Where(c => c.SubjectTrainingPrograms.Any(tp => tp.TPID == "TP002")) // Kiểm tra nếu có chương trình đào tạo nào trùng với TPID
            //.ToListAsync();
            ViewBag.TrainingProgram = await GetTPsAsync();
            return View(courses);
        }

        [HttpPost]
        public async Task<IActionResult> Search(string id_TP)
        {
            var courses = await db.Courses
            .Include(c => c.SubjectTrainingPrograms) // Bao gồm thông tin chương trình đào tạo
            .Where(c => c.SubjectTrainingPrograms.Any(tp => tp.TPID == id_TP)) // Kiểm tra nếu có chương trình đào tạo nào trùng với TPID
            .ToListAsync();
            ViewBag.TrainingProgram = await GetTPsAsync();
            return View(courses);
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
    }
}
