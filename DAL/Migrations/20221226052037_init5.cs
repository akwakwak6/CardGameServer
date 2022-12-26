using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class init5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PresiTableId",
                table: "PresiPlayers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PresiTables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PresiTables", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PresiPlayers_PresiTableId",
                table: "PresiPlayers",
                column: "PresiTableId");

            migrationBuilder.AddForeignKey(
                name: "FK_PresiPlayers_PresiTables_PresiTableId",
                table: "PresiPlayers",
                column: "PresiTableId",
                principalTable: "PresiTables",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PresiPlayers_PresiTables_PresiTableId",
                table: "PresiPlayers");

            migrationBuilder.DropTable(
                name: "PresiTables");

            migrationBuilder.DropIndex(
                name: "IX_PresiPlayers_PresiTableId",
                table: "PresiPlayers");

            migrationBuilder.DropColumn(
                name: "PresiTableId",
                table: "PresiPlayers");
        }
    }
}
