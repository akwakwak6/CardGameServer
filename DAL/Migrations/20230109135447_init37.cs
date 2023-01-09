using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class init37 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "PresiGame",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 1, 9, 14, 54, 47, 566, DateTimeKind.Local).AddTicks(4427),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 1, 9, 14, 28, 15, 308, DateTimeKind.Local).AddTicks(6768));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "PresiGame",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 1, 9, 14, 28, 15, 308, DateTimeKind.Local).AddTicks(6768),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 1, 9, 14, 54, 47, 566, DateTimeKind.Local).AddTicks(4427));
        }
    }
}
