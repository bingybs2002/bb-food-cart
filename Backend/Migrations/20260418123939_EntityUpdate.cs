using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class EntityUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Foods_ShoppingCarts_CartId",
                table: "Foods");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCarts_CustomerId",
                table: "ShoppingCarts");

            migrationBuilder.DropIndex(
                name: "IX_Foods_CartId",
                table: "Foods");

            migrationBuilder.AddColumn<int>(
                name: "FoodId",
                table: "ShoppingCarts",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_CustomerId",
                table: "ShoppingCarts",
                column: "CustomerId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_Foods_FoodId",
                table: "ShoppingCarts");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCarts_CustomerId",
                table: "ShoppingCarts");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCarts_FoodId",
                table: "ShoppingCarts");

            migrationBuilder.DropColumn(
                name: "FoodId",
                table: "ShoppingCarts");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_CustomerId",
                table: "ShoppingCarts",
                column: "CustomerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Foods_CartId",
                table: "Foods",
                column: "CartId");

            migrationBuilder.AddForeignKey(
                name: "FK_Foods_ShoppingCarts_CartId",
                table: "Foods",
                column: "CartId",
                principalTable: "ShoppingCarts",
                principalColumn: "Id");
        }
    }
}
