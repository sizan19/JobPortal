using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobPortal.Migrations
{
    /// <inheritdoc />
    public partial class addedexperienceanddeadlinedate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "jobdescriptions");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeadlineDate",
                table: "jobdescriptions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Experience",
                table: "jobdescriptions",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeadlineDate",
                table: "jobdescriptions");

            migrationBuilder.DropColumn(
                name: "Experience",
                table: "jobdescriptions");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "jobdescriptions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
