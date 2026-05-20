using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace grades_mvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialGradesSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    CourseName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Score = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Grades",
                columns: new[] { "Id", "CourseName", "Date", "Score", "StudentId" },
                values: new object[,]
                {
                    { 1, "Mathematics", new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 83m, 1 },
                    { 2, "Science", new DateTime(2025, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 57m, 2 },
                    { 3, "English", new DateTime(2025, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 56m, 3 },
                    { 4, "Mathematics", new DateTime(2025, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 76m, 4 },
                    { 5, "Science", new DateTime(2025, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 58m, 5 },
                    { 6, "English", new DateTime(2025, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 63m, 6 },
                    { 7, "Mathematics", new DateTime(2025, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 86m, 7 },
                    { 8, "Science", new DateTime(2025, 9, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 75m, 8 },
                    { 9, "English", new DateTime(2025, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 58m, 9 },
                    { 10, "Mathematics", new DateTime(2025, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 88m, 10 },
                    { 11, "Science", new DateTime(2025, 9, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 61m, 11 },
                    { 12, "English", new DateTime(2025, 9, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 62m, 12 },
                    { 13, "Mathematics", new DateTime(2025, 9, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 75m, 13 },
                    { 14, "Science", new DateTime(2025, 9, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 66m, 14 },
                    { 15, "English", new DateTime(2025, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 69m, 15 },
                    { 16, "Mathematics", new DateTime(2025, 9, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 63m, 16 },
                    { 17, "Science", new DateTime(2025, 9, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 75m, 17 },
                    { 18, "English", new DateTime(2025, 9, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 51m, 18 },
                    { 19, "Mathematics", new DateTime(2025, 9, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 90m, 19 },
                    { 20, "Science", new DateTime(2025, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 78m, 20 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Grades");
        }
    }
}
