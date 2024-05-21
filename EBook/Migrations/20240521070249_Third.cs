using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBook.Migrations
{
    /// <inheritdoc />
    public partial class Third : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthorsEf",
                columns: table => new
                {
                    AuthorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Biography = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2024, 5, 21, 12, 32, 48, 601, DateTimeKind.Local).AddTicks(23)),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2024, 5, 21, 12, 32, 48, 601, DateTimeKind.Local).AddTicks(658)),
                    isActive = table.Column<bool>(type: "bit", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ContactInfo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    SocialMedia = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorsEf", x => x.AuthorId);
                });

            migrationBuilder.CreateTable(
                name: "GeneresEf",
                columns: table => new
                {
                    GenereId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GenereName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GenereDescription = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneresEf", x => x.GenereId);
                });

            migrationBuilder.CreateTable(
                name: "EbooksEf",
                columns: table => new
                {
                    EbookId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    ISBN = table.Column<int>(type: "int", nullable: false),
                    PublicationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Publisher = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PageCount = table.Column<int>(type: "int", nullable: false),
                    AverageCounting = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2024, 5, 21, 12, 32, 48, 601, DateTimeKind.Local).AddTicks(3631)),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2024, 5, 21, 12, 32, 48, 601, DateTimeKind.Local).AddTicks(4134)),
                    GenereId = table.Column<int>(type: "int", nullable: false),
                    isAvailable = table.Column<bool>(type: "bit", nullable: false),
                    edition = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EbooksEf", x => x.EbookId);
                    table.ForeignKey(
                        name: "FK_EbooksEf_GeneresEf_GenereId",
                        column: x => x.GenereId,
                        principalTable: "GeneresEf",
                        principalColumn: "GenereId");
                });

            migrationBuilder.CreateTable(
                name: "AuthorEbooksEf",
                columns: table => new
                {
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    EbookId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorEbooksEf", x => new { x.AuthorId, x.EbookId });
                    table.ForeignKey(
                        name: "FK_AuthorEbooksEf_AuthorsEf_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AuthorsEf",
                        principalColumn: "AuthorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorEbooksEf_EbooksEf_EbookId",
                        column: x => x.EbookId,
                        principalTable: "EbooksEf",
                        principalColumn: "EbookId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorEbooksEf_EbookId",
                table: "AuthorEbooksEf",
                column: "EbookId");

            migrationBuilder.CreateIndex(
                name: "IX_EbooksEf_GenereId",
                table: "EbooksEf",
                column: "GenereId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorEbooksEf");

            migrationBuilder.DropTable(
                name: "AuthorsEf");

            migrationBuilder.DropTable(
                name: "EbooksEf");

            migrationBuilder.DropTable(
                name: "GeneresEf");
        }
    }
}
