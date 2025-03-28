using NLCS.Models.Entities;
using System.Collections.Generic;

namespace NLCS.Areas.Dean.Services
{
    public interface ITrainingProgramService
    {
        List<TrainingProgram> GetTPList();
    }
}
