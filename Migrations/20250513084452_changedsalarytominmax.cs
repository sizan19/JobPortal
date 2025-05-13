using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobPortal.Migrations
{
    /// <inheritdoc />
    public partial class changedsalarytominmax : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Salary",
                table: "jobdescriptions",
                newName: "MinSalary");

            migrationBuilder.AddColumn<decimal>(
                name: "MaxSalary",
                table: "jobdescriptions",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxSalary",
                table: "jobdescriptions");

            migrationBuilder.RenameColumn(
                name: "MinSalary",
                table: "jobdescriptions",
                newName: "Salary");
        }
    }
}
