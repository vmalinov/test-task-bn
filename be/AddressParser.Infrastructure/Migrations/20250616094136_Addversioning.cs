﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AddressParser.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Addversioning : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Version",
                table: "Addresses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "Addresses");
        }
    }
}
