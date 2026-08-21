using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OnCallApp.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Employee" },
                    { 2, "UnitManager" },
                    { 3, "Admin" }
                });

            migrationBuilder.InsertData(
                table: "Units",
                columns: new[] { "Id", "HalfDayWorkEndTime", "IsActive", "Name", "WorkEndTime", "WorkStartTime" },
                values: new object[,]
                {
                    { 1, new TimeSpan(0, 13, 0, 0, 0), true, "Yazılım", new TimeSpan(0, 18, 0, 0, 0), new TimeSpan(0, 9, 0, 0, 0) },
                    { 2, new TimeSpan(0, 13, 0, 0, 0), true, "Destek", new TimeSpan(0, 18, 0, 0, 0), new TimeSpan(0, 9, 0, 0, 0) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
