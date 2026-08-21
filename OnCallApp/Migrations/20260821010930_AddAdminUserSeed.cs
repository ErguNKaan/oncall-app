using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnCallApp.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminUserSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "Email", "FullName", "IncludeInRotation", "IsActive", "LastLoginAt", "LockoutEndAt", "MustChangePassword", "PasswordHash", "PhoneNumber", "RoleId", "UnitId" },
                values: new object[] { 1, 0, "admin@ordinatrum.com.tr", "System Admin", false, true, null, null, false, "AQAAAAIAAYagAAAAEPheZhhkwERrfyGrEkncR0sCZJGX5ikgbpIUv2tfOXsaRZD9INHGN5uXxkJhUoVnEA==", "+905550000000", 3, 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
