using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class FoodNutrition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Foods_Nutrition_NutritionId",
                table: "Foods");

            migrationBuilder.DropTable(
                name: "Nutrition");

            migrationBuilder.DropIndex(
                name: "IX_Foods_NutritionId",
                table: "Foods");

            migrationBuilder.AddColumn<int>(
                name: "Nutrition_Calories",
                table: "Foods",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Nutrition_Carbs",
                table: "Foods",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Nutrition_Id",
                table: "Foods",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Nutrition_Protein",
                table: "Foods",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nutrition_Calories",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "Nutrition_Carbs",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "Nutrition_Id",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "Nutrition_Protein",
                table: "Foods");

            migrationBuilder.CreateTable(
                name: "Nutrition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Calories = table.Column<int>(type: "integer", nullable: false),
                    Carbs = table.Column<int>(type: "integer", nullable: false),
                    Protein = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nutrition", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Foods_NutritionId",
                table: "Foods",
                column: "NutritionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Foods_Nutrition_NutritionId",
                table: "Foods",
                column: "NutritionId",
                principalTable: "Nutrition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
