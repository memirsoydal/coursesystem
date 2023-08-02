using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseSystem.Migrations
{
    public partial class AdminSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2c5e174e-3b0e-446f-86af-483d56fd7210", "2d73357c-2adc-453b-aa15-ba232b0b22e4", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "FullName", "GradeId", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "8e445865-a24d-4543-a6c6-9443d048cdb9", 0, "aebb3e52-ee87-4e13-8e67-3987975b494c", null, false, "ADMIN", "ADMIN ADMIN", null, "ADMIN", false, null, null, "MYUSER", "AQAAAAEAACcQAAAAEJuXMrwrOCYEdrY9jnBuuQmUt5Ng4nOShILOoxUxkrX4trJW/kN4w9paXeoZIhfFtg==", null, false, "5b460d77-2fef-43d7-961a-8452189727b3", false, "myuser" });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "2c5e174e-3b0e-446f-86af-483d56fd7210", "8e445865-a24d-4543-a6c6-9443d048cdb9" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2c5e174e-3b0e-446f-86af-483d56fd7210", "8e445865-a24d-4543-a6c6-9443d048cdb9" });

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9");
        }
    }
}
