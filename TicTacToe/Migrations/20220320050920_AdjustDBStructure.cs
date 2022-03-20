using Microsoft.EntityFrameworkCore.Migrations;

namespace TicTacToe.Migrations
{
    public partial class AdjustDBStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "player1",
                table: "database",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "player2",
                table: "database",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "turn",
                table: "database",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "winner",
                table: "database",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "player1",
                table: "database");

            migrationBuilder.DropColumn(
                name: "player2",
                table: "database");

            migrationBuilder.DropColumn(
                name: "turn",
                table: "database");

            migrationBuilder.DropColumn(
                name: "winner",
                table: "database");
        }
    }
}
