using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PresiPlayers_PresiTables_PresiTableId",
                table: "PresiPlayers");

            migrationBuilder.AlterColumn<int>(
                name: "PresiTableId",
                table: "PresiPlayers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PresiPlayers_PresiTables_PresiTableId",
                table: "PresiPlayers",
                column: "PresiTableId",
                principalTable: "PresiTables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PresiPlayers_PresiTables_PresiTableId",
                table: "PresiPlayers");

            migrationBuilder.AlterColumn<int>(
                name: "PresiTableId",
                table: "PresiPlayers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_PresiPlayers_PresiTables_PresiTableId",
                table: "PresiPlayers",
                column: "PresiTableId",
                principalTable: "PresiTables",
                principalColumn: "Id");
        }
    }
}
