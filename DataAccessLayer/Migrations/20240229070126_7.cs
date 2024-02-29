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
            migrationBuilder.DropForeignKey(
                name: "FK_bill_Drugs_prescriptions_PrescriptionID",
                table: "bill_Drugs");

            migrationBuilder.DropForeignKey(
                name: "FK_labReports_prescriptions_PrescriptionId",
                table: "labReports");

            migrationBuilder.DropForeignKey(
                name: "FK_prescript_Drugs_prescriptions_PrescriptionId",
                table: "prescript_Drugs");

            migrationBuilder.DropForeignKey(
                name: "FK_prescriptions_appointments_AppointmentId",
                table: "prescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_prescriptions_users_UerId",
                table: "prescriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_prescriptions",
                table: "prescriptions");

            migrationBuilder.RenameTable(
                name: "prescriptions",
                newName: "Prescription");

            migrationBuilder.RenameIndex(
                name: "IX_prescriptions_UerId",
                table: "Prescription",
                newName: "IX_Prescription_UerId");

            migrationBuilder.RenameIndex(
                name: "IX_prescriptions_AppointmentId",
                table: "Prescription",
                newName: "IX_Prescription_AppointmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Prescription",
                table: "Prescription",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_bill_Drugs_Prescription_PrescriptionID",
                table: "bill_Drugs",
                column: "PrescriptionID",
                principalTable: "Prescription",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_labReports_Prescription_PrescriptionId",
                table: "labReports",
                column: "PrescriptionId",
                principalTable: "Prescription",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_prescript_Drugs_Prescription_PrescriptionId",
                table: "prescript_Drugs",
                column: "PrescriptionId",
                principalTable: "Prescription",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prescription_appointments_AppointmentId",
                table: "Prescription",
                column: "AppointmentId",
                principalTable: "appointments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Prescription_users_UerId",
                table: "Prescription",
                column: "UerId",
                principalTable: "users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bill_Drugs_Prescription_PrescriptionID",
                table: "bill_Drugs");

            migrationBuilder.DropForeignKey(
                name: "FK_labReports_Prescription_PrescriptionId",
                table: "labReports");

            migrationBuilder.DropForeignKey(
                name: "FK_prescript_Drugs_Prescription_PrescriptionId",
                table: "prescript_Drugs");

            migrationBuilder.DropForeignKey(
                name: "FK_Prescription_appointments_AppointmentId",
                table: "Prescription");

            migrationBuilder.DropForeignKey(
                name: "FK_Prescription_users_UerId",
                table: "Prescription");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Prescription",
                table: "Prescription");

            migrationBuilder.RenameTable(
                name: "Prescription",
                newName: "prescriptions");

            migrationBuilder.RenameIndex(
                name: "IX_Prescription_UerId",
                table: "prescriptions",
                newName: "IX_prescriptions_UerId");

            migrationBuilder.RenameIndex(
                name: "IX_Prescription_AppointmentId",
                table: "prescriptions",
                newName: "IX_prescriptions_AppointmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_prescriptions",
                table: "prescriptions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_bill_Drugs_prescriptions_PrescriptionID",
                table: "bill_Drugs",
                column: "PrescriptionID",
                principalTable: "prescriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_labReports_prescriptions_PrescriptionId",
                table: "labReports",
                column: "PrescriptionId",
                principalTable: "prescriptions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_prescript_Drugs_prescriptions_PrescriptionId",
                table: "prescript_Drugs",
                column: "PrescriptionId",
                principalTable: "prescriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_prescriptions_appointments_AppointmentId",
                table: "prescriptions",
                column: "AppointmentId",
                principalTable: "appointments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_prescriptions_users_UerId",
                table: "prescriptions",
                column: "UerId",
                principalTable: "users",
                principalColumn: "Id");
        }
    }
}
