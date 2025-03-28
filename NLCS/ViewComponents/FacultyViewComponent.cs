using Microsoft.AspNetCore.Mvc;  // Cần thiết cho ViewComponent
using NLCS.Models.Entities;      // Namespace cho model Faculty và ApplicationDbContext
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NLCS.Data;  // Cần cho việc truy vấn database
namespace NLCS.ViewComponents
{
    public class FacultyViewComponent  : ViewComponent
    {
        private readonly ApplicationDbContext db;
        public FacultyViewComponent(ApplicationDbContext db)
        {
            this.db = db;  
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var faculties = await db.Faculties.ToListAsync();
            return View(faculties);
        }
    }
}
