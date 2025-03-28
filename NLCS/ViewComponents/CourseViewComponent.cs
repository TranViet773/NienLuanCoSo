using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLCS.Data;

namespace NLCS.ViewComponents
{
    public class CourseViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext db;
        public CourseViewComponent(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await db.Courses
                .OrderBy(t => Guid.NewGuid())
                .Take(3)
                .ToListAsync();
            return View(result);
        }
    }
}
