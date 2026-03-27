using Backend.Models;
using Backend.Models.Cart;
using Backend.Testing;

namespace Backend.Models.Food
{ 
    public class Food
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public string Description { get; set; } = "";
        public int NutritionId { get; set; }
        public required Nutrition Nutrition { get; set; }
        public Allergies Allergies { get; set; } = Allergies.None;
        public required FoodType FoodType { get; set; }
        public bool IsSoldOut { get; set; } = false;
        public int CartId { get; set; }
        public ShoppingCart Cart { get; set; } = null!;
    }
}
