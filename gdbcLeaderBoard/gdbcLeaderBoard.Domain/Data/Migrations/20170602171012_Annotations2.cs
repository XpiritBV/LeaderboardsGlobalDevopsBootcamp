using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace gdbcLeaderBoard.Data.Migrations
{
    public partial class Annotations2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Team_Venue_VenueId",
                table: "Team");

            migrationBuilder.RenameColumn(
                name: "VenueId",
                table: "Team",
                newName: "VenueID");

            migrationBuilder.RenameIndex(
                name: "IX_Team_VenueId",
                table: "Team",
                newName: "IX_Team_VenueID");

            migrationBuilder.AddForeignKey(
                name: "FK_Team_Venue_VenueID",
                table: "Team",
                column: "VenueID",
                principalTable: "Venue",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Team_Venue_VenueID",
                table: "Team");

            migrationBuilder.RenameColumn(
                name: "VenueID",
                table: "Team",
                newName: "VenueId");

            migrationBuilder.RenameIndex(
                name: "IX_Team_VenueID",
                table: "Team",
                newName: "IX_Team_VenueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Team_Venue_VenueId",
                table: "Team",
                column: "VenueId",
                principalTable: "Venue",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
