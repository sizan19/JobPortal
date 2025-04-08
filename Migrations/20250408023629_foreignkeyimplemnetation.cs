using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobPortal.Migrations
{
    /// <inheritdoc />
    public partial class foreignkeyimplemnetation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "jobdescriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_jobdescriptions_CategoryId",
                table: "jobdescriptions",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_jobdescriptions_VendorId",
                table: "jobdescriptions",
                column: "VendorId");

            migrationBuilder.AddForeignKey(
                name: "FK_jobdescriptions_Categories_CategoryId",
                table: "jobdescriptions",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_jobdescriptions_VendorOrganizations_VendorId",
                table: "jobdescriptions",
                column: "VendorId",
                principalTable: "VendorOrganizations",
                principalColumn: "VendorId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_jobdescriptions_Categories_CategoryId",
                table: "jobdescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_jobdescriptions_VendorOrganizations_VendorId",
                table: "jobdescriptions");

            migrationBuilder.DropIndex(
                name: "IX_jobdescriptions_CategoryId",
                table: "jobdescriptions");

            migrationBuilder.DropIndex(
                name: "IX_jobdescriptions_VendorId",
                table: "jobdescriptions");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "jobdescriptions");
        }
    }
}
