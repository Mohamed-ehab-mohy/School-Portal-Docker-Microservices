using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace grades_mvc.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StudentId = table.Column<int>(type: "integer", nullable: false),
                    CourseName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Score = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    GradeDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Grades",
                columns: new[] { "Id", "CourseName", "GradeDate", "Notes", "Score", "StudentId" },
                values: new object[,]
                {
                    { 1, "Mathematics", new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 83m, 1 },
                    { 2, "Science", new DateTime(2025, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 57m, 2 },
                    { 3, "English", new DateTime(2025, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 56m, 3 },
                    { 4, "Mathematics", new DateTime(2025, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 76m, 4 },
                    { 5, "Science", new DateTime(2025, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 58m, 5 },
                    { 6, "English", new DateTime(2025, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 63m, 6 },
                    { 7, "Mathematics", new DateTime(2025, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 86m, 7 },
                    { 8, "Science", new DateTime(2025, 9, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 75m, 8 },
                    { 9, "English", new DateTime(2025, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 58m, 9 },
                    { 10, "Mathematics", new DateTime(2025, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 88m, 10 },
                    { 11, "Science", new DateTime(2025, 9, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 61m, 11 },
                    { 12, "English", new DateTime(2025, 9, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 62m, 12 },
                    { 13, "Mathematics", new DateTime(2025, 9, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 75m, 13 },
                    { 14, "Science", new DateTime(2025, 9, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 66m, 14 },
                    { 15, "English", new DateTime(2025, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 69m, 15 },
                    { 16, "Mathematics", new DateTime(2025, 9, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 63m, 16 },
                    { 17, "Science", new DateTime(2025, 9, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 75m, 17 },
                    { 18, "English", new DateTime(2025, 9, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 51m, 18 },
                    { 19, "Mathematics", new DateTime(2025, 9, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 90m, 19 },
                    { 20, "Science", new DateTime(2025, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 78m, 20 }
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
