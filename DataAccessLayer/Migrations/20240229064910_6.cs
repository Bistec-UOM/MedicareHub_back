using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecordId",
                table: "reportFields");

            migrationBuilder.RenameColumn(
                name: "Time",
                table: "labReports",
                newName: "DateTime");

            migrationBuilder.AddColumn<int>(
                name: "ReportFieldId",
                table: "records",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReportFieldId",
                table: "records");

            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "labReports",
                newName: "Time");

            migrationBuilder.AddColumn<int>(
                name: "RecordId",
                table: "reportFields",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
