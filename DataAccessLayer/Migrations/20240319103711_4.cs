using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_prescriptions_cashiers_CashierId",
                table: "prescriptions");

            migrationBuilder.AlterColumn<float>(
                name: "Total",
                table: "prescriptions",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<int>(
                name: "CashierId",
                table: "prescriptions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_prescriptions_cashiers_CashierId",
                table: "prescriptions",
                column: "CashierId",
                principalTable: "cashiers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_prescriptions_cashiers_CashierId",
                table: "prescriptions");

            migrationBuilder.AlterColumn<float>(
                name: "Total",
                table: "prescriptions",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CashierId",
                table: "prescriptions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_prescriptions_cashiers_CashierId",
                table: "prescriptions",
                column: "CashierId",
                principalTable: "cashiers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
