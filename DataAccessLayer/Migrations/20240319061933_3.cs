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
            migrationBuilder.DropForeignKey(
                name: "FK_appointments_Doctor_DoctorId",
                table: "appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_appointments_Receptionist_RecepId",
                table: "appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Cashier_users_UserId",
                table: "Cashier");

            migrationBuilder.DropForeignKey(
                name: "FK_Doctor_users_UserId",
                table: "Doctor");

            migrationBuilder.DropForeignKey(
                name: "FK_LabAssistant_users_UserId",
                table: "LabAssistant");

            migrationBuilder.DropForeignKey(
                name: "FK_labReports_LabAssistant_LbAstID",
                table: "labReports");

            migrationBuilder.DropForeignKey(
                name: "FK_prescriptions_Cashier_CashierId",
                table: "prescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Receptionist_users_UserId",
                table: "Receptionist");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Receptionist",
                table: "Receptionist");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LabAssistant",
                table: "LabAssistant");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Doctor",
                table: "Doctor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cashier",
                table: "Cashier");

            migrationBuilder.RenameTable(
                name: "Receptionist",
                newName: "receptionists");

            migrationBuilder.RenameTable(
                name: "LabAssistant",
                newName: "labAssistants");

            migrationBuilder.RenameTable(
                name: "Doctor",
                newName: "doctors");

            migrationBuilder.RenameTable(
                name: "Cashier",
                newName: "cashiers");

            migrationBuilder.RenameIndex(
                name: "IX_Receptionist_UserId",
                table: "receptionists",
                newName: "IX_receptionists_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_LabAssistant_UserId",
                table: "labAssistants",
                newName: "IX_labAssistants_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Doctor_UserId",
                table: "doctors",
                newName: "IX_doctors_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Cashier_UserId",
                table: "cashiers",
                newName: "IX_cashiers_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_receptionists",
                table: "receptionists",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_labAssistants",
                table: "labAssistants",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_doctors",
                table: "doctors",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_cashiers",
                table: "cashiers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_appointments_doctors_DoctorId",
                table: "appointments",
                column: "DoctorId",
                principalTable: "doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_appointments_receptionists_RecepId",
                table: "appointments",
                column: "RecepId",
                principalTable: "receptionists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_cashiers_users_UserId",
                table: "cashiers",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_doctors_users_UserId",
                table: "doctors",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_labAssistants_users_UserId",
                table: "labAssistants",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_labReports_labAssistants_LbAstID",
                table: "labReports",
                column: "LbAstID",
                principalTable: "labAssistants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_prescriptions_cashiers_CashierId",
                table: "prescriptions",
                column: "CashierId",
                principalTable: "cashiers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_receptionists_users_UserId",
                table: "receptionists",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_appointments_doctors_DoctorId",
                table: "appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_appointments_receptionists_RecepId",
                table: "appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_cashiers_users_UserId",
                table: "cashiers");

            migrationBuilder.DropForeignKey(
                name: "FK_doctors_users_UserId",
                table: "doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_labAssistants_users_UserId",
                table: "labAssistants");

            migrationBuilder.DropForeignKey(
                name: "FK_labReports_labAssistants_LbAstID",
                table: "labReports");

            migrationBuilder.DropForeignKey(
                name: "FK_prescriptions_cashiers_CashierId",
                table: "prescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_receptionists_users_UserId",
                table: "receptionists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_receptionists",
                table: "receptionists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_labAssistants",
                table: "labAssistants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_doctors",
                table: "doctors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_cashiers",
                table: "cashiers");

            migrationBuilder.RenameTable(
                name: "receptionists",
                newName: "Receptionist");

            migrationBuilder.RenameTable(
                name: "labAssistants",
                newName: "LabAssistant");

            migrationBuilder.RenameTable(
                name: "doctors",
                newName: "Doctor");

            migrationBuilder.RenameTable(
                name: "cashiers",
                newName: "Cashier");

            migrationBuilder.RenameIndex(
                name: "IX_receptionists_UserId",
                table: "Receptionist",
                newName: "IX_Receptionist_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_labAssistants_UserId",
                table: "LabAssistant",
                newName: "IX_LabAssistant_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_doctors_UserId",
                table: "Doctor",
                newName: "IX_Doctor_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_cashiers_UserId",
                table: "Cashier",
                newName: "IX_Cashier_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Receptionist",
                table: "Receptionist",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LabAssistant",
                table: "LabAssistant",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Doctor",
                table: "Doctor",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cashier",
                table: "Cashier",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_appointments_Doctor_DoctorId",
                table: "appointments",
                column: "DoctorId",
                principalTable: "Doctor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_appointments_Receptionist_RecepId",
                table: "appointments",
                column: "RecepId",
                principalTable: "Receptionist",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cashier_users_UserId",
                table: "Cashier",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Doctor_users_UserId",
                table: "Doctor",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LabAssistant_users_UserId",
                table: "LabAssistant",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_labReports_LabAssistant_LbAstID",
                table: "labReports",
                column: "LbAstID",
                principalTable: "LabAssistant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_prescriptions_Cashier_CashierId",
                table: "prescriptions",
                column: "CashierId",
                principalTable: "Cashier",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Receptionist_users_UserId",
                table: "Receptionist",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
