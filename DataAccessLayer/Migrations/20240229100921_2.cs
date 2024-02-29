using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_appointments_users_UserId",
                table: "appointments");

            migrationBuilder.DropIndex(
                name: "IX_appointments_UserId",
                table: "appointments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "appointments");

            migrationBuilder.CreateIndex(
                name: "IX_appointments_DoctorId",
                table: "appointments",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_appointments_RecepId",
                table: "appointments",
                column: "RecepId");

            migrationBuilder.AddForeignKey(
                name: "FK_appointments_users_DoctorId",
                table: "appointments",
                column: "DoctorId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_appointments_users_RecepId",
                table: "appointments",
                column: "RecepId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_appointments_users_DoctorId",
                table: "appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_appointments_users_RecepId",
                table: "appointments");

            migrationBuilder.DropIndex(
                name: "IX_appointments_DoctorId",
                table: "appointments");

            migrationBuilder.DropIndex(
                name: "IX_appointments_RecepId",
                table: "appointments");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "appointments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_appointments_UserId",
                table: "appointments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_appointments_users_UserId",
                table: "appointments",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id");
        }
    }
}
