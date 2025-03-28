using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NLCS.Migrations
{
    /// <inheritdoc />
    public partial class initialmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Manager",
                columns: table => new
                {
                    Manager_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Manager_Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Manager_Sex = table.Column<bool>(type: "bit", nullable: false),
                    Manager_BornDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Manager_UserName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Manager_Password = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Manager_Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Manager_PhoneNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Manager_Information = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manager", x => x.Manager_Id);
                });

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    Student_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Student_Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Student_Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Student_DoB = table.Column<DateOnly>(type: "date", nullable: false),
                    Gender = table.Column<bool>(type: "bit", nullable: false),
                    Student_UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Student_Password = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Student_PhoneNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Student_Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.Student_Id);
                });

            migrationBuilder.CreateTable(
                name: "Subject",
                columns: table => new
                {
                    Subject_Id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Subject_Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Subject_Image = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Subject_Time = table.Column<int>(type: "int", nullable: false),
                    Subject_Quantity = table.Column<int>(type: "int", nullable: false),
                    Subject_Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject_Place = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Subject_DoiTuong = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subject", x => x.Subject_Id);
                });

            migrationBuilder.CreateTable(
                name: "Faculty",
                columns: table => new
                {
                    Faculty_Id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Faculty_Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Faculty_Description = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Faculty_Image = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Faculty_ManagerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faculty", x => x.Faculty_Id);
                    table.ForeignKey(
                        name: "FK_Faculty_Manager_Faculty_ManagerId",
                        column: x => x.Faculty_ManagerId,
                        principalTable: "Manager",
                        principalColumn: "Manager_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DangKys",
                columns: table => new
                {
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    SubjectID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Year = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Semester = table.Column<int>(type: "int", nullable: false),
                    isHoanThanh = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DangKys", x => new { x.SubjectID, x.StudentID });
                    table.ForeignKey(
                        name: "FK_DangKys_Student_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "Student_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DangKys_Subject_SubjectID",
                        column: x => x.SubjectID,
                        principalTable: "Subject",
                        principalColumn: "Subject_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Teacher",
                columns: table => new
                {
                    Teacher_Id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Teacher_Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Teacher_Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Teacher_Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Teacher_Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Teacher_Degree = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Teacher_Image = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Teacher_PhoneNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false),
                    isDisplay = table.Column<bool>(type: "bit", nullable: false),
                    Teacher_FacultyID = table.Column<string>(type: "nvarchar(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teacher", x => x.Teacher_Id);
                    table.ForeignKey(
                        name: "FK_Teacher_Faculty_Teacher_FacultyID",
                        column: x => x.Teacher_FacultyID,
                        principalTable: "Faculty",
                        principalColumn: "Faculty_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingPrograms",
                columns: table => new
                {
                    TP_Id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TP_Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TP_Student = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TP_Leader = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TP_Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TP_TrainingForm = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TP_Target = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    TP_Desciption = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    TP_FacultyId = table.Column<string>(type: "nvarchar(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingPrograms", x => x.TP_Id);
                    table.ForeignKey(
                        name: "FK_TrainingPrograms_Faculty_TP_FacultyId",
                        column: x => x.TP_FacultyId,
                        principalTable: "Faculty",
                        principalColumn: "Faculty_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GiangDays",
                columns: table => new
                {
                    TeacherID = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    SubjectID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Year = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Semester = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiangDays", x => new { x.TeacherID, x.SubjectID });
                    table.ForeignKey(
                        name: "FK_GiangDays_Subject_SubjectID",
                        column: x => x.SubjectID,
                        principalTable: "Subject",
                        principalColumn: "Subject_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GiangDays_Teacher_TeacherID",
                        column: x => x.TeacherID,
                        principalTable: "Teacher",
                        principalColumn: "Teacher_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubjectTeacher",
                columns: table => new
                {
                    SubjectsSubject_Id = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    TeachersTeacher_Id = table.Column<string>(type: "nvarchar(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectTeacher", x => new { x.SubjectsSubject_Id, x.TeachersTeacher_Id });
                    table.ForeignKey(
                        name: "FK_SubjectTeacher_Subject_SubjectsSubject_Id",
                        column: x => x.SubjectsSubject_Id,
                        principalTable: "Subject",
                        principalColumn: "Subject_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectTeacher_Teacher_TeachersTeacher_Id",
                        column: x => x.TeachersTeacher_Id,
                        principalTable: "Teacher",
                        principalColumn: "Teacher_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubjectTrainingProgram",
                columns: table => new
                {
                    SubjectsSubject_Id = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    TrainingProgramsTP_Id = table.Column<string>(type: "nvarchar(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectTrainingProgram", x => new { x.SubjectsSubject_Id, x.TrainingProgramsTP_Id });
                    table.ForeignKey(
                        name: "FK_SubjectTrainingProgram_Subject_SubjectsSubject_Id",
                        column: x => x.SubjectsSubject_Id,
                        principalTable: "Subject",
                        principalColumn: "Subject_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectTrainingProgram_TrainingPrograms_TrainingProgramsTP_Id",
                        column: x => x.TrainingProgramsTP_Id,
                        principalTable: "TrainingPrograms",
                        principalColumn: "TP_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubjectTrainingPrograms",
                columns: table => new
                {
                    TPID = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    SubjectID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectTrainingPrograms", x => new { x.TPID, x.SubjectID });
                    table.ForeignKey(
                        name: "FK_SubjectTrainingPrograms_Subject_SubjectID",
                        column: x => x.SubjectID,
                        principalTable: "Subject",
                        principalColumn: "Subject_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectTrainingPrograms_TrainingPrograms_TPID",
                        column: x => x.TPID,
                        principalTable: "TrainingPrograms",
                        principalColumn: "TP_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DangKys_StudentID",
                table: "DangKys",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_Faculty_Faculty_ManagerId",
                table: "Faculty",
                column: "Faculty_ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_GiangDays_SubjectID",
                table: "GiangDays",
                column: "SubjectID");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectTeacher_TeachersTeacher_Id",
                table: "SubjectTeacher",
                column: "TeachersTeacher_Id");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectTrainingProgram_TrainingProgramsTP_Id",
                table: "SubjectTrainingProgram",
                column: "TrainingProgramsTP_Id");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectTrainingPrograms_SubjectID",
                table: "SubjectTrainingPrograms",
                column: "SubjectID");

            migrationBuilder.CreateIndex(
                name: "IX_Teacher_Teacher_FacultyID",
                table: "Teacher",
                column: "Teacher_FacultyID");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPrograms_TP_FacultyId",
                table: "TrainingPrograms",
                column: "TP_FacultyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DangKys");

            migrationBuilder.DropTable(
                name: "GiangDays");

            migrationBuilder.DropTable(
                name: "SubjectTeacher");

            migrationBuilder.DropTable(
                name: "SubjectTrainingProgram");

            migrationBuilder.DropTable(
                name: "SubjectTrainingPrograms");

            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
                name: "Teacher");

            migrationBuilder.DropTable(
                name: "Subject");

            migrationBuilder.DropTable(
                name: "TrainingPrograms");

            migrationBuilder.DropTable(
                name: "Faculty");

            migrationBuilder.DropTable(
                name: "Manager");
        }
    }
}
