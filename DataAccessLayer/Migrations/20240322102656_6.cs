using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "doctorId",
                table: "unable_Dates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_unable_Dates_doctorId",
                table: "unable_Dates",
                column: "doctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_unable_Dates_doctors_doctorId",
                table: "unable_Dates",
                column: "doctorId",
                principalTable: "doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_unable_Dates_doctors_doctorId",
                table: "unable_Dates");

            migrationBuilder.DropIndex(
                name: "IX_unable_Dates_doctorId",
                table: "unable_Dates");

            migrationBuilder.DropColumn(
                name: "doctorId",
                table: "unable_Dates");
        }
    }
}
