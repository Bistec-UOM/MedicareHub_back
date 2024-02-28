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
            migrationBuilder.DropColumn(
                name: "FieldId",
                table: "records");

            migrationBuilder.AddColumn<int>(
                name: "RecordId",
                table: "reportFields",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_reportFields_RecordId",
                table: "reportFields",
                column: "RecordId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_reportFields_records_RecordId",
                table: "reportFields",
                column: "RecordId",
                principalTable: "records",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reportFields_records_RecordId",
                table: "reportFields");

            migrationBuilder.DropIndex(
                name: "IX_reportFields_RecordId",
                table: "reportFields");

            migrationBuilder.DropColumn(
                name: "RecordId",
                table: "reportFields");

            migrationBuilder.AddColumn<int>(
                name: "FieldId",
                table: "records",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
