﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_appointments_users_DoctorId",
                table: "appointments");

            migrationBuilder.AddForeignKey(
                name: "FK_appointments_users_DoctorId",
                table: "appointments",
                column: "DoctorId",
                principalTable: "users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_appointments_users_DoctorId",
                table: "appointments");

            migrationBuilder.AddForeignKey(
                name: "FK_appointments_users_DoctorId",
                table: "appointments",
                column: "DoctorId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
