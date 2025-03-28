using Microsoft.AspNetCore.Mvc;  // Cần thiết cho ViewComponent
using NLCS.Models.Entities;      // Namespace cho model Faculty và ApplicationDbContext
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NLCS.Data;
namespace NLCS.ViewComponents
{
    public class TeacherViewComponent:ViewComponent
    {
        private readonly ApplicationDbContext db;
        public TeacherViewComponent(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var teachers = await db.Teachers
                .OrderBy(t => Guid.NewGuid())
                .Take(4)
                .ToListAsync();
            return View(teachers); // Trả về View và truyền danh sách giáo viên
        }
    }
}
