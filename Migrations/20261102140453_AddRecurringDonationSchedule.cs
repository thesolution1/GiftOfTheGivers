using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GiftOfTheGivers.Migrations
{
    /// <inheritdoc />
    public partial class AddRecurringDonationSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RecurringDay",
                table: "Donations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecurringMonths",
                table: "Donations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ContactRequests",
                columns: table => new
                {
                    ContactRequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsContacted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactRequests", x => x.ContactRequestId);
                });

            migrationBuilder.CreateTable(
                name: "UpcomingProjectRequests",
                columns: table => new
                {
                    UpcomingProjectRequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Skills = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Availability = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsProcessed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpcomingProjectRequests", x => x.UpcomingProjectRequestId);
                });

            migrationBuilder.CreateTable(
                name: "VolunteerRequests",
                columns: table => new
                {
                    VolunteerRequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReliefProjectId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Skills = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Availability = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VolunteerRequests", x => x.VolunteerRequestId);
                    table.ForeignKey(
                        name: "FK_VolunteerRequests_ReliefProjects_ReliefProjectId",
                        column: x => x.ReliefProjectId,
                        principalTable: "ReliefProjects",
                        principalColumn: "ReliefProjectID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerRequests_ReliefProjectId",
                table: "VolunteerRequests",
                column: "ReliefProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerRequests_UserId_ReliefProjectId",
                table: "VolunteerRequests",
                columns: new[] { "UserId", "ReliefProjectId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactRequests");

            migrationBuilder.DropTable(
                name: "UpcomingProjectRequests");

            migrationBuilder.DropTable(
                name: "VolunteerRequests");

            migrationBuilder.DropColumn(
                name: "RecurringDay",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "RecurringMonths",
                table: "Donations");
        }
    }
}
