using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NLCS.Migrations
{
    /// <inheritdoc />
    public partial class Add_Field_DateOfRegistration_DangKy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isHoanThanh",
                table: "DangKys");

            migrationBuilder.AddColumn<DateOnly>(
                name: "DateOfRegistration",
                table: "DangKys",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfRegistration",
                table: "DangKys");

            migrationBuilder.AddColumn<bool>(
                name: "isHoanThanh",
                table: "DangKys",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
