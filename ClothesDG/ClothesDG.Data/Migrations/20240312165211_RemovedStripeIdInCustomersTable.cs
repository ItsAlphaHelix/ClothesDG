﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClothesDG.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedStripeIdInCustomersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StripeId",
                table: "Customers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StripeId",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
