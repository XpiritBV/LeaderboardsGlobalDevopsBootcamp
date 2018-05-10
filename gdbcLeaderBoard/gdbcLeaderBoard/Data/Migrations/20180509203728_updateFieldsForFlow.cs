using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace gdbcLeaderBoard.Data.Migrations
{
    public partial class updateFieldsForFlow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "TeamScoreItem",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HelpUrl",
                table: "Challenge",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "TeamScoreItem");

            migrationBuilder.DropColumn(
                name: "HelpUrl",
                table: "Challenge");
        }
    }
}
