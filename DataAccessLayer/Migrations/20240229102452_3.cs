using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_prescriptions_users_UserId",
                table: "prescriptions");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "prescriptions",
                newName: "CashierId");

            migrationBuilder.RenameIndex(
                name: "IX_prescriptions_UserId",
                table: "prescriptions",
                newName: "IX_prescriptions_CashierId");

            migrationBuilder.AddColumn<int>(
                name: "LbAstID",
                table: "prescriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_prescriptions_LbAstID",
                table: "prescriptions",
                column: "LbAstID");

            migrationBuilder.AddForeignKey(
                name: "FK_prescriptions_users_CashierId",
                table: "prescriptions",
                column: "CashierId",
                principalTable: "users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_prescriptions_users_LbAstID",
                table: "prescriptions",
                column: "LbAstID",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_prescriptions_users_CashierId",
                table: "prescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_prescriptions_users_LbAstID",
                table: "prescriptions");

            migrationBuilder.DropIndex(
                name: "IX_prescriptions_LbAstID",
                table: "prescriptions");

            migrationBuilder.DropColumn(
                name: "LbAstID",
                table: "prescriptions");

            migrationBuilder.RenameColumn(
                name: "CashierId",
                table: "prescriptions",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_prescriptions_CashierId",
                table: "prescriptions",
                newName: "IX_prescriptions_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_prescriptions_users_UserId",
                table: "prescriptions",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id");
        }
    }
}
