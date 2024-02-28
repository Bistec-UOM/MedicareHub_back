using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FieldName",
                table: "records");

            migrationBuilder.DropColumn(
                name: "CashierID",
                table: "prescriptions");

            migrationBuilder.DropColumn(
                name: "TestName",
                table: "labReports");

            migrationBuilder.RenameColumn(
                name: "age",
                table: "users",
                newName: "DOB");

            migrationBuilder.RenameColumn(
                name: "telephonenumber",
                table: "user_Teles",
                newName: "Telephonenumber");

            migrationBuilder.RenameColumn(
                name: "TestId",
                table: "tests",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "total",
                table: "prescriptions",
                newName: "Total");

            migrationBuilder.RenameColumn(
                name: "telephonenumber",
                table: "Patient_Teles",
                newName: "Telephonenumber");

            migrationBuilder.RenameColumn(
                name: "amount",
                table: "bill_Drugs",
                newName: "Amount");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "user_Teles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FieldId",
                table: "records",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LabReportId",
                table: "records",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReportId",
                table: "records",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CachierID",
                table: "prescriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UerId",
                table: "prescriptions",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Period",
                table: "prescript_Drugs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "PatientId",
                table: "Patient_Teles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TestId",
                table: "labReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Amount",
                table: "bill_Drugs",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.CreateIndex(
                name: "IX_user_Teles_UserId",
                table: "user_Teles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_records_LabReportId",
                table: "records",
                column: "LabReportId");

            migrationBuilder.CreateIndex(
                name: "IX_prescriptions_UerId",
                table: "prescriptions",
                column: "UerId");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_Teles_PatientId",
                table: "Patient_Teles",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_labReports_TestId",
                table: "labReports",
                column: "TestId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_labReports_tests_TestId",
                table: "labReports",
                column: "TestId",
                principalTable: "tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Patient_Teles_patients_PatientId",
                table: "Patient_Teles",
                column: "PatientId",
                principalTable: "patients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_prescriptions_users_UerId",
                table: "prescriptions",
                column: "UerId",
                principalTable: "users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_records_labReports_LabReportId",
                table: "records",
                column: "LabReportId",
                principalTable: "labReports",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_user_Teles_users_UserId",
                table: "user_Teles",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_labReports_tests_TestId",
                table: "labReports");

            migrationBuilder.DropForeignKey(
                name: "FK_Patient_Teles_patients_PatientId",
                table: "Patient_Teles");

            migrationBuilder.DropForeignKey(
                name: "FK_prescriptions_users_UerId",
                table: "prescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_records_labReports_LabReportId",
                table: "records");

            migrationBuilder.DropForeignKey(
                name: "FK_user_Teles_users_UserId",
                table: "user_Teles");

            migrationBuilder.DropIndex(
                name: "IX_user_Teles_UserId",
                table: "user_Teles");

            migrationBuilder.DropIndex(
                name: "IX_records_LabReportId",
                table: "records");

            migrationBuilder.DropIndex(
                name: "IX_prescriptions_UerId",
                table: "prescriptions");

            migrationBuilder.DropIndex(
                name: "IX_Patient_Teles_PatientId",
                table: "Patient_Teles");

            migrationBuilder.DropIndex(
                name: "IX_labReports_TestId",
                table: "labReports");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "user_Teles");

            migrationBuilder.DropColumn(
                name: "FieldId",
                table: "records");

            migrationBuilder.DropColumn(
                name: "LabReportId",
                table: "records");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "records");

            migrationBuilder.DropColumn(
                name: "CachierID",
                table: "prescriptions");

            migrationBuilder.DropColumn(
                name: "UerId",
                table: "prescriptions");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "Patient_Teles");

            migrationBuilder.DropColumn(
                name: "TestId",
                table: "labReports");

            migrationBuilder.RenameColumn(
                name: "DOB",
                table: "users",
                newName: "age");

            migrationBuilder.RenameColumn(
                name: "Telephonenumber",
                table: "user_Teles",
                newName: "telephonenumber");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "tests",
                newName: "TestId");

            migrationBuilder.RenameColumn(
                name: "Total",
                table: "prescriptions",
                newName: "total");

            migrationBuilder.RenameColumn(
                name: "Telephonenumber",
                table: "Patient_Teles",
                newName: "telephonenumber");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "bill_Drugs",
                newName: "amount");

            migrationBuilder.AddColumn<string>(
                name: "FieldName",
                table: "records",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CashierID",
                table: "prescriptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Period",
                table: "prescript_Drugs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TestName",
                table: "labReports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<float>(
                name: "amount",
                table: "bill_Drugs",
                type: "real",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
