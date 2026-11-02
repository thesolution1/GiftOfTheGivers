using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GiftOfTheGivers.Migrations
{
    /// <inheritdoc />
    public partial class AddStoredDonationTaxCertificates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TaxCertificateDonorEmail",
                table: "Donations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaxCertificateDonorName",
                table: "Donations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TaxCertificateIssuedAt",
                table: "Donations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaxCertificateNumber",
                table: "Donations",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaxCertificateDonorEmail",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "TaxCertificateDonorName",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "TaxCertificateIssuedAt",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "TaxCertificateNumber",
                table: "Donations");
        }
    }
}
