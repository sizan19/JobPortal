using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobPortal.Migrations
{
    /// <inheritdoc />
    public partial class fixingjobdescriptionforbasicneeds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VendorImage",
                table: "VendorOrganizations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "jobdescriptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "jobdescriptions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeltetedBy",
                table: "jobdescriptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeltetedDate",
                table: "jobdescriptions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JobVacancy",
                table: "jobdescriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "jobdescriptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "jobdescriptions",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VendorImage",
                table: "VendorOrganizations");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "jobdescriptions");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "jobdescriptions");

            migrationBuilder.DropColumn(
                name: "DeltetedBy",
                table: "jobdescriptions");

            migrationBuilder.DropColumn(
                name: "DeltetedDate",
                table: "jobdescriptions");

            migrationBuilder.DropColumn(
                name: "JobVacancy",
                table: "jobdescriptions");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "jobdescriptions");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "jobdescriptions");
        }
    }
}
