using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pronia.Migrations
{
    /// <inheritdoc />
    public partial class AddRatingToProducts_v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
       name: "Rating",
       table: "Products",
       type: "int",
       nullable: false,
       defaultValue: 0); // mövcud satırlar üçün default dəyər
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
       name: "Rating",
       table: "Products");

        }
    }
}
