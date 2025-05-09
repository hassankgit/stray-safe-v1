using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StraySafe.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixingSightingConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SightingDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Species = table.Column<string>(type: "text", nullable: true),
                    Breed = table.Column<string>(type: "text", nullable: true),
                    Age = table.Column<int>(type: "integer", nullable: false),
                    Sex = table.Column<int>(type: "integer", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    LastSpotted = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: true),
                    Tags_Status = table.Column<int>(type: "integer", nullable: true),
                    Tags_Behavior = table.Column<int>(type: "integer", nullable: true),
                    Tags_Health = table.Column<int>(type: "integer", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    SubmittedById = table.Column<string>(type: "text", nullable: false),
                    SubmittedByName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SightingDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SightingPreviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Species = table.Column<string>(type: "text", nullable: true),
                    Breed = table.Column<string>(type: "text", nullable: true),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    LastSpotted = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Coordinates_Latitude = table.Column<double>(type: "double precision", nullable: true),
                    Coordinates_Longitude = table.Column<double>(type: "double precision", nullable: true),
                    SubmittedById = table.Column<string>(type: "text", nullable: false),
                    SightingDetailId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SightingPreviews", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SightingDetails");

            migrationBuilder.DropTable(
                name: "SightingPreviews");
        }
    }
}
