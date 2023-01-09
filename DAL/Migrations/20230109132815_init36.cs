using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class init36 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PresiGameId",
                table: "PresiPlayers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PresiGame",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2023, 1, 9, 14, 28, 15, 308, DateTimeKind.Local).AddTicks(6768)),
                    PresiTableId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PresiGame", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PresiPlayers_PresiGameId",
                table: "PresiPlayers",
                column: "PresiGameId");

            migrationBuilder.AddForeignKey(
                name: "FK_PresiPlayers_PresiGame_PresiGameId",
                table: "PresiPlayers",
                column: "PresiGameId",
                principalTable: "PresiGame",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PresiPlayers_PresiGame_PresiGameId",
                table: "PresiPlayers");

            migrationBuilder.DropTable(
                name: "PresiGame");

            migrationBuilder.DropIndex(
                name: "IX_PresiPlayers_PresiGameId",
                table: "PresiPlayers");

            migrationBuilder.DropColumn(
                name: "PresiGameId",
                table: "PresiPlayers");
        }
    }
}
