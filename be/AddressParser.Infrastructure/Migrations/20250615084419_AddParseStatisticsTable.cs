using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AddressParser.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddParseStatisticsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ParseStatistics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalParsed = table.Column<int>(type: "integer", nullable: false),
                    TotalFailed = table.Column<int>(type: "integer", nullable: false),
                    LastSuccess = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastFailure = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AvgParseDurationMs = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParseStatistics", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParseStatistics");
        }
    }
}
