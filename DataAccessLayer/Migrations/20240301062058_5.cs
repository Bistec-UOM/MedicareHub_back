using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_prescriptions_users_LbAstID",
                table: "prescriptions");

            migrationBuilder.DropIndex(
                name: "IX_prescriptions_LbAstID",
                table: "prescriptions");

            migrationBuilder.DropColumn(
                name: "LbAstID",
                table: "prescriptions");

            migrationBuilder.AddColumn<int>(
                name: "LbAstID",
                table: "labReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_labReports_LbAstID",
                table: "labReports",
                column: "LbAstID");

            migrationBuilder.AddForeignKey(
                name: "FK_labReports_users_LbAstID",
                table: "labReports",
                column: "LbAstID",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_labReports_users_LbAstID",
                table: "labReports");

            migrationBuilder.DropIndex(
                name: "IX_labReports_LbAstID",
                table: "labReports");

            migrationBuilder.DropColumn(
                name: "LbAstID",
                table: "labReports");

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
                name: "FK_prescriptions_users_LbAstID",
                table: "prescriptions",
                column: "LbAstID",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
