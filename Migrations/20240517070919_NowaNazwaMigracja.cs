using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gry_Slownikowe.Migrations
{
    /// <inheritdoc />
    public partial class NowaNazwaMigracja : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nick = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Login = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Ranks = table.Column<int>(type: "int", nullable: false),
                    AccountCreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Krzyzowki",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Win = table.Column<int>(type: "int", nullable: false),
                    Loss = table.Column<int>(type: "int", nullable: false),
                    GameTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    GameData = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()"),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Krzyzowki", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Krzyzowki_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Slownikowo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Win = table.Column<int>(type: "int", nullable: false),
                    Loss = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GameTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    GameData = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()"),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slownikowo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Slownikowo_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wisielecs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Win = table.Column<int>(type: "int", nullable: false),
                    Loss = table.Column<int>(type: "int", nullable: false),
                    GameTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    GameData = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()"),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wisielecs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wisielecs_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wordle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Win = table.Column<int>(type: "int", nullable: false),
                    Loss = table.Column<int>(type: "int", nullable: false),
                    GameTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    GameData = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()"),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wordle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wordle_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Zgadywanki",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Win = table.Column<int>(type: "int", nullable: false),
                    Loss = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GameTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    GameData = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()"),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zgadywanki", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Zgadywanki_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Krzyzowki_UserId",
                table: "Krzyzowki",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Slownikowo_UserId",
                table: "Slownikowo",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Wisielecs_UserId",
                table: "Wisielecs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Wordle_UserId",
                table: "Wordle",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Zgadywanki_UserId",
                table: "Zgadywanki",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Krzyzowki");

            migrationBuilder.DropTable(
                name: "Slownikowo");

            migrationBuilder.DropTable(
                name: "Wisielecs");

            migrationBuilder.DropTable(
                name: "Wordle");

            migrationBuilder.DropTable(
                name: "Zgadywanki");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
