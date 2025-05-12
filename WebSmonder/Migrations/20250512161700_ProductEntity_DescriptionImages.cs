using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebSmonder.Migrations
{
    /// <inheritdoc />
    public partial class ProductEntity_DescriptionImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string[]>(
                name: "DescriptionImages",
                table: "tbl_products",
                type: "text[]",
                nullable: false,
                defaultValue: new string[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionImages",
                table: "tbl_products");
        }
    }
}
