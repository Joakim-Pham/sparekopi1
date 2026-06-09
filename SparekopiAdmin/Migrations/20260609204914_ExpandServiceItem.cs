using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SparekopiAdmin.Migrations
{
    /// <inheritdoc />
    public partial class ExpandServiceItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "ServiceItems",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Features",
                table: "ServiceItems",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "ServiceItems",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "ServiceItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PricePrefix",
                table: "ServiceItems",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PriceUnit",
                table: "ServiceItems",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Section",
                table: "ServiceItems",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShortDescription",
                table: "ServiceItems",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "ServiceItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "ServiceItems");

            migrationBuilder.DropColumn(
                name: "Features",
                table: "ServiceItems");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "ServiceItems");

            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "ServiceItems");

            migrationBuilder.DropColumn(
                name: "PricePrefix",
                table: "ServiceItems");

            migrationBuilder.DropColumn(
                name: "PriceUnit",
                table: "ServiceItems");

            migrationBuilder.DropColumn(
                name: "Section",
                table: "ServiceItems");

            migrationBuilder.DropColumn(
                name: "ShortDescription",
                table: "ServiceItems");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "ServiceItems");
        }
    }
}
