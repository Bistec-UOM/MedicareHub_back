using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_bill_Drugs_DrugID",
                table: "bill_Drugs");

            migrationBuilder.CreateIndex(
                name: "IX_bill_Drugs_DrugID",
                table: "bill_Drugs",
                column: "DrugID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_bill_Drugs_DrugID",
                table: "bill_Drugs");

            migrationBuilder.CreateIndex(
                name: "IX_bill_Drugs_DrugID",
                table: "bill_Drugs",
                column: "DrugID",
                unique: true);
        }
    }
}
