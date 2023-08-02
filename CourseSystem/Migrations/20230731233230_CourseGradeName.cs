using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseSystem.Migrations
{
    public partial class CourseGradeName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CourseGradeName",
                table: "CourseGrades",
                type: "longtext",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210",
                column: "ConcurrencyStamp",
                value: "4b54583b-ad0f-4c9b-912f-c344f276963a");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a4d94644-d98a-4af3-be6a-2c17fc873806", "AQAAAAEAACcQAAAAELFAe27vDyNp5/fEOBYomT87wNkFU7nG2OkX65VX4ShalItEl3zdcl8rgx03eKtYYA==", "b69616b2-d551-4ff8-8340-7f9ab72fdf66" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseGradeName",
                table: "CourseGrades");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210",
                column: "ConcurrencyStamp",
                value: "2d73357c-2adc-453b-aa15-ba232b0b22e4");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "aebb3e52-ee87-4e13-8e67-3987975b494c", "AQAAAAEAACcQAAAAEJuXMrwrOCYEdrY9jnBuuQmUt5Ng4nOShILOoxUxkrX4trJW/kN4w9paXeoZIhfFtg==", "5b460d77-2fef-43d7-961a-8452189727b3" });
        }
    }
}
