using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace E_commerce.DAL.Migrations
{
    /// <inheritdoc />
    public partial class migrationforerrorfix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "39a61b39-163e-4b33-896e-a2e57d85eab8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "caebd75c-b366-4fbb-86a0-a6499fb1daaa");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "04db2643-a53f-4b33-920f-3ccc9563aac9", "1", "Admin", "ADMIN" },
                    { "f66fe0a8-8603-4a6e-bacc-cbff7d8c2ef8", "2", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "04db2643-a53f-4b33-920f-3ccc9563aac9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f66fe0a8-8603-4a6e-bacc-cbff7d8c2ef8");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "39a61b39-163e-4b33-896e-a2e57d85eab8", "1", "Admin", "ADMIN" },
                    { "caebd75c-b366-4fbb-86a0-a6499fb1daaa", "2", "User", "USER" }
                });
        }
    }
}
