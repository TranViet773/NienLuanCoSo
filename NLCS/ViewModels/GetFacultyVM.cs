using NLCS.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NLCS.ViewModels
{
    public class GetFacultyVM
    {
        public string Faculty_Id { get; set; }
       
        public string Faculty_Name { get; set; }

        public string Faculty_Description { get; set; }
       
        public string Faculty_Image { get; set; }

        
    }
}
