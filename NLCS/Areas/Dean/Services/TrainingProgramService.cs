using NLCS.Areas.Dean.Repositories;
using NLCS.Data;
using NLCS.Models.Entities;

namespace NLCS.Areas.Dean.Services
{
    public class TrainingProgramService : ITrainingProgramService
    {
        private readonly ApplicationDbContext db;
        private readonly DeanRepository deanRepository;
        private readonly IHttpContextAccessor httpContextAccessor;
        public TrainingProgramService(ApplicationDbContext db, DeanRepository deanRepository, IHttpContextAccessor httpContextAccessor)
        {
            this.db = db;
            this.deanRepository = deanRepository;
            this.httpContextAccessor = httpContextAccessor;
        }
        public List<TrainingProgram> GetTPList()
        {
            var user = httpContextAccessor.HttpContext.User;
            string id_khoa = deanRepository.getCurrentTeacher(user).Teacher_FacultyID;
            return db.TrainingPrograms.Where(t => t.TP_FacultyId == id_khoa).ToList();
        }
    }
}
