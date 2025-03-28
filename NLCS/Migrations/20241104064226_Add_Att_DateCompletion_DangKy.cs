using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NLCS.Migrations
{
    /// <inheritdoc />
    public partial class Add_Att_DateCompletion_DangKy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "DateOfCompletion",
                table: "DangKys",
                type: "date",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfCompletion",
                table: "DangKys");
        }
    }
}
