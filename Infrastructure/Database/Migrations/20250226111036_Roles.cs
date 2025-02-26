using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Roles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("7b8c76e5-636f-4ebd-9254-b3845539409c"), "Client" },
                    { new Guid("bb8deb6a-f586-461d-8dbb-ad0fbe2a05bc"), "Admin" },
                    { new Guid("cc4c1f40-f6a1-4ba2-9029-3476f92b6438"), "Specialist" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("7b8c76e5-636f-4ebd-9254-b3845539409c"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("bb8deb6a-f586-461d-8dbb-ad0fbe2a05bc"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("cc4c1f40-f6a1-4ba2-9029-3476f92b6438"));
        }
    }
}
