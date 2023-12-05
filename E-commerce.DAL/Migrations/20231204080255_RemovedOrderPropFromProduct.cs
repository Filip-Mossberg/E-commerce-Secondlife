using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace E_commerce.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RemovedOrderPropFromProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0183f8db-5876-406f-bf4c-0db5d80c0732");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3833e74c-7c99-47cc-8421-5ffdb723710f");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "997567ee-0464-4b1a-a8a1-d4823c0c2275", "2", "User", "USER" },
                    { "b3cc29a4-0b1f-4cb8-8fce-f6b7766a0807", "1", "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "997567ee-0464-4b1a-a8a1-d4823c0c2275");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b3cc29a4-0b1f-4cb8-8fce-f6b7766a0807");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0183f8db-5876-406f-bf4c-0db5d80c0732", "1", "Admin", "ADMIN" },
                    { "3833e74c-7c99-47cc-8421-5ffdb723710f", "2", "User", "USER" }
                });
        }
    }
}
