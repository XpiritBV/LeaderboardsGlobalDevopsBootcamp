using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace gdbcLeaderBoard.Data.Migrations
{
    public partial class venueAdminIDNotRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Venue_AspNetUsers_VenueAdminID",
                table: "Venue");

            migrationBuilder.AlterColumn<string>(
                name: "VenueAdminID",
                table: "Venue",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_Venue_AspNetUsers_VenueAdminID",
                table: "Venue",
                column: "VenueAdminID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Venue_AspNetUsers_VenueAdminID",
                table: "Venue");

            migrationBuilder.AlterColumn<string>(
                name: "VenueAdminID",
                table: "Venue",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Venue_AspNetUsers_VenueAdminID",
                table: "Venue",
                column: "VenueAdminID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
