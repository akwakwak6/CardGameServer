using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PresiPlayers");

            migrationBuilder.DropTable(
                name: "PresiTables");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "PresiPlayers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PresiTableId = table.Column<int>(type: "int", nullable: true),
                    Pseudo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PresiPlayers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PresiPlayers_PresiTables_PresiTableId",
                        column: x => x.PresiTableId,
                        principalTable: "PresiTables",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PresiPlayers_PresiTableId",
                table: "PresiPlayers",
                column: "PresiTableId");
        }
    }
}
