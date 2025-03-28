namespace NLCS.Areas.Dean.ViewModels
{
    public class DangKyVM
    {
        public string Subject_Id { get; set; }
        public string Subject_Name { get; set; }
        public int Student_Id { get; set; }
        public string Student_Email { get; set; }
        public string Student_Name { get; set; }
        public string Student_Username { get; set; }
        public string Year { get; set; }
        public int Semester { get; set; }
        public int DangKy_Status { get; set; }
        public DateOnly DateOfRegistration { get; set; }
    }
}
