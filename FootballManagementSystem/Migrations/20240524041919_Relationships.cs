using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class Relationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Clubs_AwayClubId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Clubs_HomeClubId",
                table: "Matches");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Clubs_AwayClubId",
                table: "Matches",
                column: "AwayClubId",
                principalTable: "Clubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Clubs_HomeClubId",
                table: "Matches",
                column: "HomeClubId",
                principalTable: "Clubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Clubs_AwayClubId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Clubs_HomeClubId",
                table: "Matches");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Clubs_AwayClubId",
                table: "Matches",
                column: "AwayClubId",
                principalTable: "Clubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Clubs_HomeClubId",
                table: "Matches",
                column: "HomeClubId",
                principalTable: "Clubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
