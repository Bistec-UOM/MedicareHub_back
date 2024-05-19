using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _23 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_labReports_labAssistants_LbAstID",
                table: "labReports");

            migrationBuilder.AlterColumn<int>(
                name: "LbAstID",
                table: "labReports",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_labReports_labAssistants_LbAstID",
                table: "labReports",
                column: "LbAstID",
                principalTable: "labAssistants",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_labReports_labAssistants_LbAstID",
                table: "labReports");

            migrationBuilder.AlterColumn<int>(
                name: "LbAstID",
                table: "labReports",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_labReports_labAssistants_LbAstID",
                table: "labReports",
                column: "LbAstID",
                principalTable: "labAssistants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
