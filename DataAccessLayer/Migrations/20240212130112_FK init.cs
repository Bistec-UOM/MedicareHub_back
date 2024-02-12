using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class FKinit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TestName",
                table: "reportFileds");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "tests",
                newName: "TestId");

            migrationBuilder.AddColumn<int>(
                name: "TestId",
                table: "reportFileds",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_reportFileds_TestId",
                table: "reportFileds",
                column: "TestId");

            migrationBuilder.AddForeignKey(
                name: "FK_reportFileds_tests_TestId",
                table: "reportFileds",
                column: "TestId",
                principalTable: "tests",
                principalColumn: "TestId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reportFileds_tests_TestId",
                table: "reportFileds");

            migrationBuilder.DropIndex(
                name: "IX_reportFileds_TestId",
                table: "reportFileds");

            migrationBuilder.DropColumn(
                name: "TestId",
                table: "reportFileds");

            migrationBuilder.RenameColumn(
                name: "TestId",
                table: "tests",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "TestName",
                table: "reportFileds",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
