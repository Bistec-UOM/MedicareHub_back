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
                name: "FK_reportFields_records_RecordId",
                table: "reportFields");

            migrationBuilder.DropIndex(
                name: "IX_reportFields_RecordId",
                table: "reportFields");

            migrationBuilder.AddColumn<int>(
                name: "ReportFieldsId",
                table: "records",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_records_ReportFieldsId",
                table: "records",
                column: "ReportFieldsId");

            migrationBuilder.AddForeignKey(
                name: "FK_records_reportFields_ReportFieldsId",
                table: "records",
                column: "ReportFieldsId",
                principalTable: "reportFields",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_records_reportFields_ReportFieldsId",
                table: "records");

            migrationBuilder.DropIndex(
                name: "IX_records_ReportFieldsId",
                table: "records");

            migrationBuilder.DropColumn(
                name: "ReportFieldsId",
                table: "records");

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
    }
}
