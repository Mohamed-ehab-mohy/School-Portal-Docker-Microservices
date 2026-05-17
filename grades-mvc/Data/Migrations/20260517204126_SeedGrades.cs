using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace grades_mvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedGrades : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Grades",
                columns: new[] { "Id", "Date", "Score", "StudentId", "Subject" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 83m, 1, "Mathematics" },
                    { 2, new DateTime(2025, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 57m, 2, "Science" },
                    { 3, new DateTime(2025, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 56m, 3, "English" },
                    { 4, new DateTime(2025, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 76m, 4, "Mathematics" },
                    { 5, new DateTime(2025, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 58m, 5, "Science" },
                    { 6, new DateTime(2025, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 63m, 6, "English" },
                    { 7, new DateTime(2025, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 86m, 7, "Mathematics" },
                    { 8, new DateTime(2025, 9, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 75m, 8, "Science" },
                    { 9, new DateTime(2025, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 58m, 9, "English" },
                    { 10, new DateTime(2025, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 88m, 10, "Mathematics" },
                    { 11, new DateTime(2025, 9, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 61m, 11, "Science" },
                    { 12, new DateTime(2025, 9, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 62m, 12, "English" },
                    { 13, new DateTime(2025, 9, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 75m, 13, "Mathematics" },
                    { 14, new DateTime(2025, 9, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 66m, 14, "Science" },
                    { 15, new DateTime(2025, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 69m, 15, "English" },
                    { 16, new DateTime(2025, 9, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 63m, 16, "Mathematics" },
                    { 17, new DateTime(2025, 9, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 75m, 17, "Science" },
                    { 18, new DateTime(2025, 9, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 51m, 18, "English" },
                    { 19, new DateTime(2025, 9, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 90m, 19, "Mathematics" },
                    { 20, new DateTime(2025, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 78m, 20, "Science" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 20);
        }
    }
}
