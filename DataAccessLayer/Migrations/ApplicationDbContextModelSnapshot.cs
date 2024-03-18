﻿// <auto-generated />
using System;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataAccessLayer.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Models.Appointment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("DoctorId")
                        .HasColumnType("int");

                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.Property<int>("RecepId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DoctorId");

                    b.HasIndex("PatientId");

                    b.HasIndex("RecepId");

                    b.ToTable("appointments");
                });

            modelBuilder.Entity("Models.Bill_drug", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<int>("DrugID")
                        .HasColumnType("int");

                    b.Property<int>("PrescriptionID")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DrugID")
                        .IsUnique();

                    b.HasIndex("PrescriptionID");

                    b.ToTable("bill_Drugs");
                });

            modelBuilder.Entity("Models.Cashier", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Cashier");
                });

            modelBuilder.Entity("Models.Doctor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Doctor");
                });

            modelBuilder.Entity("Models.Drug", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Avaliable")
                        .HasColumnType("int");

                    b.Property<string>("BrandN")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GenericN")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<int>("Weight")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("drugs");
                });

            modelBuilder.Entity("Models.LabAssistant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("LabAssistant");
                });

            modelBuilder.Entity("Models.LabReport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("LbAstID")
                        .HasColumnType("int");

                    b.Property<int>("PrescriptionID")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TestId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LbAstID");

                    b.HasIndex("PrescriptionID");

                    b.HasIndex("TestId");

                    b.ToTable("labReports");
                });

            modelBuilder.Entity("Models.Patient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DOB")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NIC")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("patients");
                });

            modelBuilder.Entity("Models.Patient_Teles", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("PatientId")
                        .HasColumnType("int");

                    b.Property<int>("Telephonenumber")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PatientId");

                    b.ToTable("Patient_Teles");
                });

            modelBuilder.Entity("Models.Prescript_drug", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("GenericN")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Period")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PrescriptionId")
                        .HasColumnType("int");

                    b.Property<string>("Unit")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Weight")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("PrescriptionId");

                    b.ToTable("prescript_Drugs");
                });

            modelBuilder.Entity("Models.Prescription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AppointmentID")
                        .HasColumnType("int");

                    b.Property<int>("CashierId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<float>("Total")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("AppointmentID")
                        .IsUnique();

                    b.HasIndex("CashierId");

                    b.ToTable("prescriptions");
                });

            modelBuilder.Entity("Models.Receptionist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Receptionist");
                });

            modelBuilder.Entity("Models.Record", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("LabReportId")
                        .HasColumnType("int");

                    b.Property<int>("ReportFieldId")
                        .HasColumnType("int");

                    b.Property<float>("Result")
                        .HasColumnType("real");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LabReportId");

                    b.HasIndex("ReportFieldId");

                    b.ToTable("records");
                });

            modelBuilder.Entity("Models.ReportFields", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Fieldname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<short>("Index")
                        .HasColumnType("smallint");

                    b.Property<float>("MaxRef")
                        .HasColumnType("real");

                    b.Property<float>("MinRef")
                        .HasColumnType("real");

                    b.Property<int>("TestId")
                        .HasColumnType("int");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("TestId");

                    b.ToTable("reportFields");
                });

            modelBuilder.Entity("Models.Test", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Abb")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<string>("Provider")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TestName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("tests");
                });

            modelBuilder.Entity("Models.Unable_Date", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("unable_Dates");
                });

            modelBuilder.Entity("Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DOB")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NIC")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Qualifications")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("users");
                });

            modelBuilder.Entity("Models.User_Tele", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Telephonenumber")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("user_Teles");
                });

            modelBuilder.Entity("Models.Appointment", b =>
                {
                    b.HasOne("Models.Doctor", "Doctor")
                        .WithMany()
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Models.Patient", "Patient")
                        .WithMany("Appointment")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Receptionist", "Recep")
                        .WithMany()
                        .HasForeignKey("RecepId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Doctor");

                    b.Navigation("Patient");

                    b.Navigation("Recep");
                });

            modelBuilder.Entity("Models.Bill_drug", b =>
                {
                    b.HasOne("Models.Drug", "Drug")
                        .WithOne("Bill_drug")
                        .HasForeignKey("Models.Bill_drug", "DrugID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Prescription", "Prescription")
                        .WithMany("Bill_drug")
                        .HasForeignKey("PrescriptionID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Drug");

                    b.Navigation("Prescription");
                });

            modelBuilder.Entity("Models.Cashier", b =>
                {
                    b.HasOne("Models.User", "User")
                        .WithOne("Cashier")
                        .HasForeignKey("Models.Cashier", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Models.Doctor", b =>
                {
                    b.HasOne("Models.User", "User")
                        .WithOne("Doctor")
                        .HasForeignKey("Models.Doctor", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Models.LabAssistant", b =>
                {
                    b.HasOne("Models.User", "User")
                        .WithOne("LabAssistant")
                        .HasForeignKey("Models.LabAssistant", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Models.LabReport", b =>
                {
                    b.HasOne("Models.LabAssistant", "LbAst")
                        .WithMany()
                        .HasForeignKey("LbAstID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Prescription", "Prescription")
                        .WithMany("LabReport")
                        .HasForeignKey("PrescriptionID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Test", "Test")
                        .WithMany("LabReport")
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LbAst");

                    b.Navigation("Prescription");

                    b.Navigation("Test");
                });

            modelBuilder.Entity("Models.Patient_Teles", b =>
                {
                    b.HasOne("Models.Patient", "Patient")
                        .WithMany("Patient_Teles")
                        .HasForeignKey("PatientId");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("Models.Prescript_drug", b =>
                {
                    b.HasOne("Models.Prescription", "Prescription")
                        .WithMany("Prescript_drug")
                        .HasForeignKey("PrescriptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Prescription");
                });

            modelBuilder.Entity("Models.Prescription", b =>
                {
                    b.HasOne("Models.Appointment", "Appointment")
                        .WithOne("Prescription")
                        .HasForeignKey("Models.Prescription", "AppointmentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Cashier", "Cashier")
                        .WithMany()
                        .HasForeignKey("CashierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Appointment");

                    b.Navigation("Cashier");
                });

            modelBuilder.Entity("Models.Receptionist", b =>
                {
                    b.HasOne("Models.User", "User")
                        .WithOne("Receptionist")
                        .HasForeignKey("Models.Receptionist", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Models.Record", b =>
                {
                    b.HasOne("Models.LabReport", "LabReport")
                        .WithMany("Records")
                        .HasForeignKey("LabReportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.ReportFields", "ReportField")
                        .WithMany("Record")
                        .HasForeignKey("ReportFieldId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LabReport");

                    b.Navigation("ReportField");
                });

            modelBuilder.Entity("Models.ReportFields", b =>
                {
                    b.HasOne("Models.Test", "Test")
                        .WithMany("ReportFields")
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Test");
                });

            modelBuilder.Entity("Models.User_Tele", b =>
                {
                    b.HasOne("Models.User", "User")
                        .WithMany("User_Tele")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Models.Appointment", b =>
                {
                    b.Navigation("Prescription");
                });

            modelBuilder.Entity("Models.Drug", b =>
                {
                    b.Navigation("Bill_drug");
                });

            modelBuilder.Entity("Models.LabReport", b =>
                {
                    b.Navigation("Records");
                });

            modelBuilder.Entity("Models.Patient", b =>
                {
                    b.Navigation("Appointment");

                    b.Navigation("Patient_Teles");
                });

            modelBuilder.Entity("Models.Prescription", b =>
                {
                    b.Navigation("Bill_drug");

                    b.Navigation("LabReport");

                    b.Navigation("Prescript_drug");
                });

            modelBuilder.Entity("Models.ReportFields", b =>
                {
                    b.Navigation("Record");
                });

            modelBuilder.Entity("Models.Test", b =>
                {
                    b.Navigation("LabReport");

                    b.Navigation("ReportFields");
                });

            modelBuilder.Entity("Models.User", b =>
                {
                    b.Navigation("Cashier");

                    b.Navigation("Doctor");

                    b.Navigation("LabAssistant");

                    b.Navigation("Receptionist");

                    b.Navigation("User_Tele");
                });
#pragma warning restore 612, 618
        }
    }
}
