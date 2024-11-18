﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClothesDG.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserProfileImageUrlToProductReviewsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserProfileImageUrl",
                table: "ProductsReviews",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserProfileImageUrl",
                table: "ProductsReviews");
        }
    }
}
