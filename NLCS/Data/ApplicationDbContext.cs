using Microsoft.EntityFrameworkCore;
using NLCS.Models.Entities;

namespace NLCS.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {
        }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Subject> Courses { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<TrainingProgram> TrainingPrograms { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<DangKy> DangKys { get; set; }
        public DbSet<TrangThaiDangKy> TrangThaiDangKys { get; set; }
        public DbSet<GiangDay> GiangDays { get; set; }
        public DbSet<SubjectTrainingProgram> SubjectTrainingPrograms { get; set; }

        //public DbSet<Student_Course> Students_Courses { get; set; }
        //public DbSet<Teacher_Course> Teacher_Courses { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Dang ky
            //modelBuilder.Entity<DangKy>()
            //    .HasKey(ms => new { ms.SubjectID, ms.StudentID });

            //modelBuilder.Entity<DangKy>()
            //    .HasOne(ms => ms.Subject)
            //    .WithMany(m => m.DangKy)
            //    .HasForeignKey(ms => ms.StudentID);

            //modelBuilder.Entity<DangKy>()
            //    .HasOne(ms => ms.Student)
            //    .WithMany(s => s.DangKy)
            //    .HasForeignKey(ms => ms.StudentID);
            ////Giang Day
            //modelBuilder.Entity<GiangDay>()
            //   .HasKey(ms => new { ms.TeacherID, ms.SubjectID });

            //modelBuilder.Entity<GiangDay>()
            //    .HasOne(ms => ms.subject)
            //    .WithMany(m => m.GiangDays)
            //    .HasForeignKey(ms => ms.TeacherID);

            //modelBuilder.Entity<GiangDay>()
            //    .HasOne(ms => ms.teacher)
            //    .WithMany(s => s.GiangDays)
            //    .HasForeignKey(ms => ms.SubjectID);

            // Cấu hình cho DangKy
            modelBuilder.Entity<DangKy>()
                .HasKey(dk => new { dk.SubjectID, dk.StudentID });

            modelBuilder.Entity<DangKy>()
                .HasOne(dk => dk.Subject)
                .WithMany(s => s.DangKy) // Sử dụng DangKys, không phải DangKy
                .HasForeignKey(dk => dk.SubjectID); // Khóa ngoại đúng là SubjectID

            modelBuilder.Entity<DangKy>()
                .HasOne(dk => dk.Student)
                .WithMany(s => s.DangKy) // Sử dụng DangKys, không phải DangKy
                .HasForeignKey(dk => dk.StudentID);

            // Cấu hình cho GiangDay
            modelBuilder.Entity<GiangDay>()
                .HasKey(gd => new { gd.TeacherID, gd.SubjectID });

            modelBuilder.Entity<GiangDay>()
                .HasOne(gd => gd.subject)
                .WithMany(s => s.GiangDays)
                .HasForeignKey(gd => gd.SubjectID); // Khóa ngoại đúng là SubjectID

            modelBuilder.Entity<GiangDay>()
                .HasOne(gd => gd.teacher)
                .WithMany(t => t.GiangDays)
                .HasForeignKey(gd => gd.TeacherID);

            // Cấu hình cho SubjectTP
            modelBuilder.Entity<SubjectTrainingProgram>()
                .HasKey(st => new { st.TPID, st.SubjectID });

            modelBuilder.Entity<SubjectTrainingProgram>()
                .HasOne(st => st.subject)
                .WithMany(s => s.SubjectTrainingPrograms)
                .HasForeignKey(st => st.SubjectID); // Khóa ngoại đúng là SubjectID

            modelBuilder.Entity<SubjectTrainingProgram>()
                .HasOne(st => st.trainingProgram)
                .WithMany(t => t.SubjectTrainingPrograms)
                .HasForeignKey(st => st.TPID);
        }

    }


}
