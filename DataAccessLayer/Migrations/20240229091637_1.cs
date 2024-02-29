using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "drugs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GenericN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BrandN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    Avaliable = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_drugs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "patients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NIC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DOB = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_patients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TestName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Abb = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    Provider = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "unable_Dates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_unable_Dates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NIC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Qualifications = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DOB = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patient_Teles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Telephonenumber = table.Column<int>(type: "int", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient_Teles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Patient_Teles_patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "patients",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "reportFields",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fieldname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Index = table.Column<short>(type: "smallint", nullable: false),
                    MinRef = table.Column<float>(type: "real", nullable: false),
                    MaxRef = table.Column<float>(type: "real", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TestId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reportFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_reportFields_tests_TestId",
                        column: x => x.TestId,
                        principalTable: "tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "appointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    RecepId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_appointments_patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_appointments_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "user_Teles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Telephonenumber = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_Teles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_Teles_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "prescriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentID = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<float>(type: "real", nullable: false),
                    CachierID = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_prescriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_prescriptions_appointments_AppointmentID",
                        column: x => x.AppointmentID,
                        principalTable: "appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_prescriptions_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "bill_Drugs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DrugID = table.Column<int>(type: "int", nullable: false),
                    PrescriptionID = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bill_Drugs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_bill_Drugs_drugs_DrugID",
                        column: x => x.DrugID,
                        principalTable: "drugs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bill_Drugs_prescriptions_PrescriptionID",
                        column: x => x.PrescriptionID,
                        principalTable: "prescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "labReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrescriptID = table.Column<int>(type: "int", nullable: false),
                    PrescriptionId = table.Column<int>(type: "int", nullable: true),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TestId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_labReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_labReports_prescriptions_PrescriptionId",
                        column: x => x.PrescriptionId,
                        principalTable: "prescriptions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_labReports_tests_TestId",
                        column: x => x.TestId,
                        principalTable: "tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "prescript_Drugs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrescriptionId = table.Column<int>(type: "int", nullable: false),
                    GenericN = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<float>(type: "real", nullable: false),
                    Period = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_prescript_Drugs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_prescript_Drugs_prescriptions_PrescriptionId",
                        column: x => x.PrescriptionId,
                        principalTable: "prescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Record",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LabReportId = table.Column<int>(type: "int", nullable: false),
                    ReportFieldId = table.Column<int>(type: "int", nullable: false),
                    Result = table.Column<float>(type: "real", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Record", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Record_labReports_LabReportId",
                        column: x => x.LabReportId,
                        principalTable: "labReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Record_reportFields_ReportFieldId",
                        column: x => x.ReportFieldId,
                        principalTable: "reportFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_appointments_PatientId",
                table: "appointments",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_appointments_UserId",
                table: "appointments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_bill_Drugs_DrugID",
                table: "bill_Drugs",
                column: "DrugID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bill_Drugs_PrescriptionID",
                table: "bill_Drugs",
                column: "PrescriptionID");

            migrationBuilder.CreateIndex(
                name: "IX_labReports_PrescriptionId",
                table: "labReports",
                column: "PrescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_labReports_TestId",
                table: "labReports",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_Teles_PatientId",
                table: "Patient_Teles",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_prescript_Drugs_PrescriptionId",
                table: "prescript_Drugs",
                column: "PrescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_prescriptions_AppointmentID",
                table: "prescriptions",
                column: "AppointmentID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_prescriptions_UserId",
                table: "prescriptions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Record_LabReportId",
                table: "Record",
                column: "LabReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Record_ReportFieldId",
                table: "Record",
                column: "ReportFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_reportFields_TestId",
                table: "reportFields",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_user_Teles_UserId",
                table: "user_Teles",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bill_Drugs");

            migrationBuilder.DropTable(
                name: "Patient_Teles");

            migrationBuilder.DropTable(
                name: "prescript_Drugs");

            migrationBuilder.DropTable(
                name: "Record");

            migrationBuilder.DropTable(
                name: "unable_Dates");

            migrationBuilder.DropTable(
                name: "user_Teles");

            migrationBuilder.DropTable(
                name: "drugs");

            migrationBuilder.DropTable(
                name: "labReports");

            migrationBuilder.DropTable(
                name: "reportFields");

            migrationBuilder.DropTable(
                name: "prescriptions");

            migrationBuilder.DropTable(
                name: "tests");

            migrationBuilder.DropTable(
                name: "appointments");

            migrationBuilder.DropTable(
                name: "patients");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
