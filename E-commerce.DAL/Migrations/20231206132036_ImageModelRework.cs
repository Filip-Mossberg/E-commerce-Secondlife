using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace E_commerce.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ImageModelRework : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "578fe86c-7c25-4a46-b77f-42d8901eb483");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6eaf73bb-8ba5-45ce-a4ec-254cd152f26b");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "39a61b39-163e-4b33-896e-a2e57d85eab8", "1", "Admin", "ADMIN" },
                    { "caebd75c-b366-4fbb-86a0-a6499fb1daaa", "2", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                    { "578fe86c-7c25-4a46-b77f-42d8901eb483", "1", "Admin", "ADMIN" },
                    { "6eaf73bb-8ba5-45ce-a4ec-254cd152f26b", "2", "User", "USER" }
                });
        }
    }
}
