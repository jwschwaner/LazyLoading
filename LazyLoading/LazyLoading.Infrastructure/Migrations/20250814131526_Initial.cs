using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LazyLoading.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    KickoffAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    HomeTeam = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    AwayTeam = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    HomeScore = table.Column<int>(type: "integer", nullable: false),
                    AwayScore = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Matches_KickoffAt",
                table: "Matches",
                column: "KickoffAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Matches");
        }
    }
}
