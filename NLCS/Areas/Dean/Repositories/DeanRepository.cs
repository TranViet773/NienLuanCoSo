using NLCS.Data;
using NLCS.Models.Entities;
using System.Security.Claims;

namespace NLCS.Areas.Dean.Repositories
{
    public class DeanRepository
    {
        private readonly ApplicationDbContext db;
        public DeanRepository(ApplicationDbContext db) {
            
                this.db = db;
            }
            public Teacher getCurrentTeacher(ClaimsPrincipal User)
            {
                var result = new Teacher();
                var userClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
                if (userClaim != null)
                {
                    var username = userClaim.Value;
                    result = db.Teachers.FirstOrDefault(m => m.Teacher_Username == username);
                }
                return result;
            }
            
        }
    }

