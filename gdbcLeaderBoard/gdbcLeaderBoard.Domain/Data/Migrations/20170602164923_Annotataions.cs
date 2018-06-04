using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace gdbcLeaderBoard.Data.Migrations
{
    public partial class Annotataions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Team_Venue_VenueId",
                table: "Team");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamScoreItem_Challenge_ChallengeId",
                table: "TeamScoreItem");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamScoreItem_Team_TeamId",
                table: "TeamScoreItem");

            migrationBuilder.DropForeignKey(
                name: "FK_Venue_AspNetUsers_VenueAdminId",
                table: "Venue");

            migrationBuilder.RenameColumn(
                name: "VenueAdminId",
                table: "Venue",
                newName: "VenueAdminID");

            migrationBuilder.RenameIndex(
                name: "IX_Venue_VenueAdminId",
                table: "Venue",
                newName: "IX_Venue_VenueAdminID");

            migrationBuilder.AlterColumn<string>(
                name: "VenueAdminID",
                table: "Venue",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Venue",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TeamId",
                table: "TeamScoreItem",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ChallengeId",
                table: "TeamScoreItem",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "VenueId",
                table: "Team",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Team",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Challenge",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Team_Venue_VenueId",
                table: "Team",
                column: "VenueId",
                principalTable: "Venue",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamScoreItem_Challenge_ChallengeId",
                table: "TeamScoreItem",
                column: "ChallengeId",
                principalTable: "Challenge",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamScoreItem_Team_TeamId",
                table: "TeamScoreItem",
                column: "TeamId",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Venue_AspNetUsers_VenueAdminID",
                table: "Venue",
                column: "VenueAdminID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Team_Venue_VenueId",
                table: "Team");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamScoreItem_Challenge_ChallengeId",
                table: "TeamScoreItem");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamScoreItem_Team_TeamId",
                table: "TeamScoreItem");

            migrationBuilder.DropForeignKey(
                name: "FK_Venue_AspNetUsers_VenueAdminID",
                table: "Venue");

            migrationBuilder.RenameColumn(
                name: "VenueAdminID",
                table: "Venue",
                newName: "VenueAdminId");

            migrationBuilder.RenameIndex(
                name: "IX_Venue_VenueAdminID",
                table: "Venue",
                newName: "IX_Venue_VenueAdminId");

            migrationBuilder.AlterColumn<string>(
                name: "VenueAdminId",
                table: "Venue",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Venue",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "TeamId",
                table: "TeamScoreItem",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "ChallengeId",
                table: "TeamScoreItem",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "VenueId",
                table: "Team",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Team",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Challenge",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_Team_Venue_VenueId",
                table: "Team",
                column: "VenueId",
                principalTable: "Venue",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamScoreItem_Challenge_ChallengeId",
                table: "TeamScoreItem",
                column: "ChallengeId",
                principalTable: "Challenge",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamScoreItem_Team_TeamId",
                table: "TeamScoreItem",
                column: "TeamId",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Venue_AspNetUsers_VenueAdminId",
                table: "Venue",
                column: "VenueAdminId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
