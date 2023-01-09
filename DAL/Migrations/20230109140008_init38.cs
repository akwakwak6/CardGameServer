using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class init38 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "PresiGame",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 1, 9, 15, 0, 8, 116, DateTimeKind.Local).AddTicks(5631),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 1, 9, 14, 54, 47, 566, DateTimeKind.Local).AddTicks(4427));

            migrationBuilder.CreateTable(
                name: "PresiPlayedCard",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    Tour = table.Column<int>(type: "int", nullable: false),
                    Card = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PresiPlayedCard", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PresiPlayedCard_PresiGame_GameId",
                        column: x => x.GameId,
                        principalTable: "PresiGame",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PresiPlayedCard_PresiPlayers_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "PresiPlayers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PresiPlayedCard_GameId",
                table: "PresiPlayedCard",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_PresiPlayedCard_PlayerId",
                table: "PresiPlayedCard",
                column: "PlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PresiPlayedCard");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "PresiGame",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 1, 9, 14, 54, 47, 566, DateTimeKind.Local).AddTicks(4427),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 1, 9, 15, 0, 8, 116, DateTimeKind.Local).AddTicks(5631));
        }
    }
}
