using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NLCS.Migrations
{
    /// <inheritdoc />
    public partial class Add_Field_Image_Student : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Student_Image",
                table: "Student",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Student_Image",
                table: "Student");
        }
    }
}
