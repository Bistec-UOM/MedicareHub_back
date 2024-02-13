using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mamasavemekt10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_drugs",
                table: "drugs");

            migrationBuilder.DropColumn(
                name: "DrugId",
                table: "drugs");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "drugs",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_drugs",
                table: "drugs",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_drugs",
                table: "drugs");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "drugs");

            migrationBuilder.AddColumn<string>(
                name: "DrugId",
                table: "drugs",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_drugs",
                table: "drugs",
                column: "DrugId");
        }
    }
}
