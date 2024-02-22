using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class reportFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reportFileds_tests_TestId",
                table: "reportFileds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_reportFileds",
                table: "reportFileds");

            migrationBuilder.RenameTable(
                name: "reportFileds",
                newName: "reportFields");

            migrationBuilder.RenameIndex(
                name: "IX_reportFileds_TestId",
                table: "reportFields",
                newName: "IX_reportFields_TestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_reportFields",
                table: "reportFields",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_reportFields_tests_TestId",
                table: "reportFields",
                column: "TestId",
                principalTable: "tests",
                principalColumn: "TestId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reportFields_tests_TestId",
                table: "reportFields");

            migrationBuilder.DropPrimaryKey(
                name: "PK_reportFields",
                table: "reportFields");

            migrationBuilder.RenameTable(
                name: "reportFields",
                newName: "reportFileds");

            migrationBuilder.RenameIndex(
                name: "IX_reportFields_TestId",
                table: "reportFileds",
                newName: "IX_reportFileds_TestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_reportFileds",
                table: "reportFileds",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_reportFileds_tests_TestId",
                table: "reportFileds",
                column: "TestId",
                principalTable: "tests",
                principalColumn: "TestId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
