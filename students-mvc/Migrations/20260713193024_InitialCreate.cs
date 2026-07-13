using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace students_mvc.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClassRooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Grade = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    RoomNumber = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Capacity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassRooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Category = table.Column<int>(type: "integer", nullable: false),
                    Link = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    IconClass = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EnrollmentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Specialization = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    HireDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StudentId = table.Column<int>(type: "integer", nullable: false),
                    ClassRoomId = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Notes = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attendances_ClassRooms_ClassRoomId",
                        column: x => x.ClassRoomId,
                        principalTable: "ClassRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Attendances_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentClasses",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "integer", nullable: false),
                    ClassRoomId = table.Column<int>(type: "integer", nullable: false),
                    AssignedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentClasses", x => new { x.StudentId, x.ClassRoomId });
                    table.ForeignKey(
                        name: "FK_StudentClasses_ClassRooms_ClassRoomId",
                        column: x => x.ClassRoomId,
                        principalTable: "ClassRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentClasses_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassTeachers",
                columns: table => new
                {
                    ClassRoomId = table.Column<int>(type: "integer", nullable: false),
                    TeacherId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassTeachers", x => new { x.ClassRoomId, x.TeacherId });
                    table.ForeignKey(
                        name: "FK_ClassTeachers_ClassRooms_ClassRoomId",
                        column: x => x.ClassRoomId,
                        principalTable: "ClassRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassTeachers_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ClassRooms",
                columns: new[] { "Id", "Capacity", "Grade", "Name", "RoomNumber" },
                values: new object[,]
                {
                    { 1, 30, "1st", "Class 1-A", "Room 101" },
                    { 2, 30, "1st", "Class 1-B", "Room 102" },
                    { 3, 35, "2nd", "Class 2-A", "Room 201" },
                    { 4, 35, "2nd", "Class 2-B", "Room 202" },
                    { 5, 40, "3rd", "Class 3-A", "Room 301" }
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "DateOfBirth", "Email", "EnrollmentDate", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1, new DateTime(2000, 2, 2, 0, 0, 0, 0, DateTimeKind.Utc), "student01@school.com", new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ahmed", "Ali" },
                    { 2, new DateTime(2000, 3, 3, 0, 0, 0, 0, DateTimeKind.Utc), "student02@school.com", new DateTime(2024, 9, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Mohamed", "Hassan" },
                    { 3, new DateTime(2000, 4, 4, 0, 0, 0, 0, DateTimeKind.Utc), "student03@school.com", new DateTime(2024, 9, 3, 0, 0, 0, 0, DateTimeKind.Utc), "Ali", "Ibrahim" },
                    { 4, new DateTime(2000, 5, 5, 0, 0, 0, 0, DateTimeKind.Utc), "student04@school.com", new DateTime(2024, 9, 4, 0, 0, 0, 0, DateTimeKind.Utc), "Omar", "Saleh" },
                    { 5, new DateTime(2000, 6, 6, 0, 0, 0, 0, DateTimeKind.Utc), "student05@school.com", new DateTime(2024, 9, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Khaled", "Nour" },
                    { 6, new DateTime(2000, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), "student06@school.com", new DateTime(2024, 9, 6, 0, 0, 0, 0, DateTimeKind.Utc), "Youssef", "Adel" },
                    { 7, new DateTime(2000, 8, 8, 0, 0, 0, 0, DateTimeKind.Utc), "student07@school.com", new DateTime(2024, 9, 7, 0, 0, 0, 0, DateTimeKind.Utc), "Hassan", "Karim" },
                    { 8, new DateTime(2000, 9, 9, 0, 0, 0, 0, DateTimeKind.Utc), "student08@school.com", new DateTime(2024, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), "Mahmoud", "Fahmy" },
                    { 9, new DateTime(2000, 10, 10, 0, 0, 0, 0, DateTimeKind.Utc), "student09@school.com", new DateTime(2024, 9, 9, 0, 0, 0, 0, DateTimeKind.Utc), "Tarek", "Saeed" },
                    { 10, new DateTime(2000, 11, 11, 0, 0, 0, 0, DateTimeKind.Utc), "student10@school.com", new DateTime(2024, 9, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Sami", "Mansour" },
                    { 11, new DateTime(2000, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), "student11@school.com", new DateTime(2024, 9, 11, 0, 0, 0, 0, DateTimeKind.Utc), "Ahmed", "Ali" },
                    { 12, new DateTime(2001, 1, 13, 0, 0, 0, 0, DateTimeKind.Utc), "student12@school.com", new DateTime(2024, 9, 12, 0, 0, 0, 0, DateTimeKind.Utc), "Mohamed", "Hassan" },
                    { 13, new DateTime(2001, 2, 14, 0, 0, 0, 0, DateTimeKind.Utc), "student13@school.com", new DateTime(2024, 9, 13, 0, 0, 0, 0, DateTimeKind.Utc), "Ali", "Ibrahim" },
                    { 14, new DateTime(2001, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), "student14@school.com", new DateTime(2024, 9, 14, 0, 0, 0, 0, DateTimeKind.Utc), "Omar", "Saleh" },
                    { 15, new DateTime(2001, 4, 16, 0, 0, 0, 0, DateTimeKind.Utc), "student15@school.com", new DateTime(2024, 9, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Khaled", "Nour" },
                    { 16, new DateTime(2001, 5, 17, 0, 0, 0, 0, DateTimeKind.Utc), "student16@school.com", new DateTime(2024, 9, 16, 0, 0, 0, 0, DateTimeKind.Utc), "Youssef", "Adel" },
                    { 17, new DateTime(2001, 6, 18, 0, 0, 0, 0, DateTimeKind.Utc), "student17@school.com", new DateTime(2024, 9, 17, 0, 0, 0, 0, DateTimeKind.Utc), "Hassan", "Karim" },
                    { 18, new DateTime(2001, 7, 19, 0, 0, 0, 0, DateTimeKind.Utc), "student18@school.com", new DateTime(2024, 9, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Mahmoud", "Fahmy" },
                    { 19, new DateTime(2001, 8, 20, 0, 0, 0, 0, DateTimeKind.Utc), "student19@school.com", new DateTime(2024, 9, 19, 0, 0, 0, 0, DateTimeKind.Utc), "Tarek", "Saeed" },
                    { 20, new DateTime(2001, 9, 21, 0, 0, 0, 0, DateTimeKind.Utc), "student20@school.com", new DateTime(2024, 9, 20, 0, 0, 0, 0, DateTimeKind.Utc), "Sami", "Mansour" }
                });

            migrationBuilder.InsertData(
                table: "Teachers",
                columns: new[] { "Id", "Email", "FirstName", "HireDate", "LastName", "Phone", "Specialization" },
                values: new object[,]
                {
                    { 1, "fatma.hassan@school.com", "Fatma", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Hassan", "01012345678", "Mathematics" },
                    { 2, "mahmoud.ali@school.com", "Mahmoud", new DateTime(2019, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Ali", "01098765432", "Physics" },
                    { 3, "nour.eldin@school.com", "Nour", new DateTime(2021, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), "El-Din", "01155566677", "Arabic" },
                    { 4, "sara.mahmoud@school.com", "Sara", new DateTime(2022, 8, 20, 0, 0, 0, 0, DateTimeKind.Utc), "Mahmoud", "01233344455", "English" },
                    { 5, "khaled.nour@school.com", "Khaled", new DateTime(2018, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Nour", "01077788899", "Chemistry" }
                });

            migrationBuilder.InsertData(
                table: "ClassTeachers",
                columns: new[] { "ClassRoomId", "TeacherId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 3 },
                    { 2, 2 },
                    { 2, 4 },
                    { 3, 1 },
                    { 3, 5 },
                    { 4, 3 },
                    { 5, 2 },
                    { 5, 4 }
                });

            migrationBuilder.InsertData(
                table: "StudentClasses",
                columns: new[] { "ClassRoomId", "StudentId", "AssignedDate" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 1, 2, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, 3, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, 4, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, 5, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, 6, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, 7, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, 8, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, 9, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, 10, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_ClassRoomId",
                table: "Attendances",
                column: "ClassRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_StudentId",
                table: "Attendances",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassTeachers_TeacherId",
                table: "ClassTeachers",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentClasses_ClassRoomId",
                table: "StudentClasses",
                column: "ClassRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_Email",
                table: "Students",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropTable(
                name: "ClassTeachers");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "StudentClasses");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropTable(
                name: "ClassRooms");

            migrationBuilder.DropTable(
                name: "Students");
        }
    }
}
