using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceApp.Auth.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Logins",
                columns: new[] { "Id", "Age", "City", "Password", "Pincode", "RoleId", "UserId", "Username" },
                values: new object[] { 121, 21, "Madurai", "$2b$10$iMiekxYY0FWjkytubt3GG.6rX/UzWPlbalo5qnpJ1Ke0V.H7BdbLi", 1234, 1, "U123", "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Logins",
                keyColumn: "Id",
                keyValue: 121);
        }
    }
}
