using NLCS.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NLCS.Areas.Dean.ViewModels
{
    public class SearchCourseByTP
    {
        public string TPID { get; set; }

        public string TP_name { get; set; }
        public string SubjectID { get; set; }
    }
}
