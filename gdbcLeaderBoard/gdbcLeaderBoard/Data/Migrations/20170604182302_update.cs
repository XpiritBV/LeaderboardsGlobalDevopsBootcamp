using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace gdbcLeaderBoard.Data.Migrations
{
    public partial class update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamScoreItem_Challenge_ChallengeId",
                table: "TeamScoreItem");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamScoreItem_Team_TeamId",
                table: "TeamScoreItem");

            migrationBuilder.RenameColumn(
                name: "TeamId",
                table: "TeamScoreItem",
                newName: "TeamID");

            migrationBuilder.RenameColumn(
                name: "ChallengeId",
                table: "TeamScoreItem",
                newName: "ChallengeID");

            migrationBuilder.RenameIndex(
                name: "IX_TeamScoreItem_TeamId",
                table: "TeamScoreItem",
                newName: "IX_TeamScoreItem_TeamID");

            migrationBuilder.RenameIndex(
                name: "IX_TeamScoreItem_ChallengeId",
                table: "TeamScoreItem",
                newName: "IX_TeamScoreItem_ChallengeID");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamScoreItem_Challenge_ChallengeID",
                table: "TeamScoreItem",
                column: "ChallengeID",
                principalTable: "Challenge",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamScoreItem_Team_TeamID",
                table: "TeamScoreItem",
                column: "TeamID",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamScoreItem_Challenge_ChallengeID",
                table: "TeamScoreItem");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamScoreItem_Team_TeamID",
                table: "TeamScoreItem");

            migrationBuilder.RenameColumn(
                name: "TeamID",
                table: "TeamScoreItem",
                newName: "TeamId");

            migrationBuilder.RenameColumn(
                name: "ChallengeID",
                table: "TeamScoreItem",
                newName: "ChallengeId");

            migrationBuilder.RenameIndex(
                name: "IX_TeamScoreItem_TeamID",
                table: "TeamScoreItem",
                newName: "IX_TeamScoreItem_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_TeamScoreItem_ChallengeID",
                table: "TeamScoreItem",
                newName: "IX_TeamScoreItem_ChallengeId");

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
        }
    }
}
