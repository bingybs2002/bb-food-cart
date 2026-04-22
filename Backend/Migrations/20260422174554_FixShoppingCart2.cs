using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class FixShoppingCart2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_Foods_FoodId",
                table: "ShoppingCarts");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCarts_FoodId",
                table: "ShoppingCarts");

            migrationBuilder.DropColumn(
                name: "FoodId",
                table: "ShoppingCarts");

            migrationBuilder.DropColumn(
                name: "CartId",
                table: "Foods");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FoodId",
                table: "ShoppingCarts",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CartId",
                table: "Foods",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_FoodId",
                table: "ShoppingCarts",
                column: "FoodId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_Foods_FoodId",
                table: "ShoppingCarts",
                column: "FoodId",
                principalTable: "Foods",
                principalColumn: "Id");
        }
    }
}
