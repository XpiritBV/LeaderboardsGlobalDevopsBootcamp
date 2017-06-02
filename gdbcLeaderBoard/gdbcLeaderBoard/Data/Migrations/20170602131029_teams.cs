using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace gdbcLeaderBoard.Data.Migrations
{
    public partial class teams : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Venue",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    VenueAdminId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Venue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Venue_AspNetUsers_VenueAdminId",
                        column: x => x.VenueAdminId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    VenueId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Team_Venue_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venue",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeamScoreItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChallengeId = table.Column<int>(nullable: true),
                    TeamId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamScoreItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamScoreItem_Challenge_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "Challenge",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeamScoreItem_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Team_VenueId",
                table: "Team",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamScoreItem_ChallengeId",
                table: "TeamScoreItem",
                column: "ChallengeId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamScoreItem_TeamId",
                table: "TeamScoreItem",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Venue_VenueAdminId",
                table: "Venue",
                column: "VenueAdminId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeamScoreItem");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropTable(
                name: "Venue");
        }
    }
}
