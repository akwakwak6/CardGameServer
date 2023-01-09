using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class init39 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "PresiGame",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 1, 9, 15, 37, 37, 554, DateTimeKind.Local).AddTicks(7023),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 1, 9, 15, 0, 8, 116, DateTimeKind.Local).AddTicks(5631));

            migrationBuilder.CreateIndex(
                name: "IX_Users_Pseudo",
                table: "Users",
                column: "Pseudo",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Pseudo",
                table: "Users");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "PresiGame",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 1, 9, 15, 0, 8, 116, DateTimeKind.Local).AddTicks(5631),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 1, 9, 15, 37, 37, 554, DateTimeKind.Local).AddTicks(7023));
        }
    }
}
