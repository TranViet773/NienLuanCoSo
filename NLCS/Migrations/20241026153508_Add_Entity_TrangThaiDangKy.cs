using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NLCS.Migrations
{
    /// <inheritdoc />
    public partial class Add_Entity_TrangThaiDangKy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Subject_Content",
                table: "Subject",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DangKy_StatusId",
                table: "DangKys",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TrangThaiDangKys",
                columns: table => new
                {
                    Status_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status_Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrangThaiDangKys", x => x.Status_Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DangKys_DangKy_StatusId",
                table: "DangKys",
                column: "DangKy_StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_DangKys_TrangThaiDangKys_DangKy_StatusId",
                table: "DangKys",
                column: "DangKy_StatusId",
                principalTable: "TrangThaiDangKys",
                principalColumn: "Status_Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DangKys_TrangThaiDangKys_DangKy_StatusId",
                table: "DangKys");

            migrationBuilder.DropTable(
                name: "TrangThaiDangKys");

            migrationBuilder.DropIndex(
                name: "IX_DangKys_DangKy_StatusId",
                table: "DangKys");

            migrationBuilder.DropColumn(
                name: "Subject_Content",
                table: "Subject");

            migrationBuilder.DropColumn(
                name: "DangKy_StatusId",
                table: "DangKys");
        }
    }
}
