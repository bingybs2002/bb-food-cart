using Backend.Models.Cart;

namespace Backend.Models.Foods
{
    public class Food
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string Description { get; set; } = "";
        public int NutritionId { get; set; }
        public Nutrition? Nutrition { get; set; } = null!;
        public Allergies Allergies { get; set; } = Allergies.None;
        public required FoodType FoodType { get; set; }
        public bool IsSoldOut { get; set; } = false;
        public List<CartItem> CartItems { get; set; } = [];
    }
}
