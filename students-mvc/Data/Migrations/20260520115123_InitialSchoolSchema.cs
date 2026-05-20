using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace students_mvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchoolSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EnrollmentDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "DateOfBirth", "Email", "EnrollmentDate", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1, new DateTime(2000, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "student01@school.com", new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ahmed", "Ali" },
                    { 2, new DateTime(2000, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "student02@school.com", new DateTime(2024, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mohamed", "Hassan" },
                    { 3, new DateTime(2000, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "student03@school.com", new DateTime(2024, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ali", "Ibrahim" },
                    { 4, new DateTime(2000, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "student04@school.com", new DateTime(2024, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Omar", "Saleh" },
                    { 5, new DateTime(2000, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "student05@school.com", new DateTime(2024, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Khaled", "Nour" },
                    { 6, new DateTime(2000, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "student06@school.com", new DateTime(2024, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Youssef", "Adel" },
                    { 7, new DateTime(2000, 8, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "student07@school.com", new DateTime(2024, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hassan", "Karim" },
                    { 8, new DateTime(2000, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "student08@school.com", new DateTime(2024, 9, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mahmoud", "Fahmy" },
                    { 9, new DateTime(2000, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "student09@school.com", new DateTime(2024, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tarek", "Saeed" },
                    { 10, new DateTime(2000, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "student10@school.com", new DateTime(2024, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sami", "Mansour" },
                    { 11, new DateTime(2000, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "student11@school.com", new DateTime(2024, 9, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ahmed", "Ali" },
                    { 12, new DateTime(2001, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "student12@school.com", new DateTime(2024, 9, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mohamed", "Hassan" },
                    { 13, new DateTime(2001, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "student13@school.com", new DateTime(2024, 9, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ali", "Ibrahim" },
                    { 14, new DateTime(2001, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "student14@school.com", new DateTime(2024, 9, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Omar", "Saleh" },
                    { 15, new DateTime(2001, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "student15@school.com", new DateTime(2024, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Khaled", "Nour" },
                    { 16, new DateTime(2001, 5, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "student16@school.com", new DateTime(2024, 9, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Youssef", "Adel" },
                    { 17, new DateTime(2001, 6, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "student17@school.com", new DateTime(2024, 9, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hassan", "Karim" },
                    { 18, new DateTime(2001, 7, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "student18@school.com", new DateTime(2024, 9, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mahmoud", "Fahmy" },
                    { 19, new DateTime(2001, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "student19@school.com", new DateTime(2024, 9, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tarek", "Saeed" },
                    { 20, new DateTime(2001, 9, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "student20@school.com", new DateTime(2024, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sami", "Mansour" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Students");
        }
    }
}
