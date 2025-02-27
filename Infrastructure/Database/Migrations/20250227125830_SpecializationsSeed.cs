using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class SpecializationsSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Specializations",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("9b6f2c4d-7a3e-4e99-8d1a-5c7a3b2d6f4e"), "Masseur" },
                    { new Guid("a3f5d1b2-6c3e-4b99-8e1a-9f6d4c7b2e3f"), "Plumber" },
                    { new Guid("b7e2c4d9-1a5f-4c88-97e3-d6a9b5f4c1e8"), "Hairdresser" },
                    { new Guid("c1d4f5e6-3b7a-4c99-8e2a-5d9b6f4c7a3e"), "Electrician" },
                    { new Guid("d6f2b3c4-5e7a-4c99-8d1a-9b4f7e6c5a3d"), "Carpenter" },
                    { new Guid("e1a3d5f7-6c4b-4e99-8d2a-9f5c7b2a3d6e"), "Mechanic" },
                    { new Guid("f5c7a3b2-6d4e-4e99-8d1a-9b6f2c4d7a3e"), "Painter" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Specializations",
                keyColumn: "Id",
                keyValue: new Guid("9b6f2c4d-7a3e-4e99-8d1a-5c7a3b2d6f4e"));

            migrationBuilder.DeleteData(
                table: "Specializations",
                keyColumn: "Id",
                keyValue: new Guid("a3f5d1b2-6c3e-4b99-8e1a-9f6d4c7b2e3f"));

            migrationBuilder.DeleteData(
                table: "Specializations",
                keyColumn: "Id",
                keyValue: new Guid("b7e2c4d9-1a5f-4c88-97e3-d6a9b5f4c1e8"));

            migrationBuilder.DeleteData(
                table: "Specializations",
                keyColumn: "Id",
                keyValue: new Guid("c1d4f5e6-3b7a-4c99-8e2a-5d9b6f4c7a3e"));

            migrationBuilder.DeleteData(
                table: "Specializations",
                keyColumn: "Id",
                keyValue: new Guid("d6f2b3c4-5e7a-4c99-8d1a-9b4f7e6c5a3d"));

            migrationBuilder.DeleteData(
                table: "Specializations",
                keyColumn: "Id",
                keyValue: new Guid("e1a3d5f7-6c4b-4e99-8d2a-9f5c7b2a3d6e"));

            migrationBuilder.DeleteData(
                table: "Specializations",
                keyColumn: "Id",
                keyValue: new Guid("f5c7a3b2-6d4e-4e99-8d1a-9b6f2c4d7a3e"));
        }
    }
}
