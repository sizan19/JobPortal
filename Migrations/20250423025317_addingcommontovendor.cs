using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobPortal.Migrations
{
    /// <inheritdoc />
    public partial class addingcommontovendor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "VendorOrganizations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "VendorOrganizations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeltetedBy",
                table: "VendorOrganizations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeltetedDate",
                table: "VendorOrganizations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "VendorOrganizations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "VendorOrganizations",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "VendorOrganizations");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "VendorOrganizations");

            migrationBuilder.DropColumn(
                name: "DeltetedBy",
                table: "VendorOrganizations");

            migrationBuilder.DropColumn(
                name: "DeltetedDate",
                table: "VendorOrganizations");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "VendorOrganizations");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "VendorOrganizations");
        }
    }
}
