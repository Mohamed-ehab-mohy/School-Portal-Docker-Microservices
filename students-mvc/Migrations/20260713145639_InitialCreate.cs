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
                name: "Students");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
