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
                name: "FK_prescriptions_users_CashierId",
                table: "prescriptions");

            migrationBuilder.DropColumn(
                name: "CachierID",
                table: "prescriptions");

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
                name: "FK_prescriptions_users_CashierId",
                table: "prescriptions",
                column: "CashierId",
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

            migrationBuilder.AlterColumn<int>(
                name: "CashierId",
                table: "prescriptions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CachierID",
                table: "prescriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_prescriptions_users_CashierId",
                table: "prescriptions",
                column: "CashierId",
                principalTable: "users",
                principalColumn: "Id");
        }
    }
}
