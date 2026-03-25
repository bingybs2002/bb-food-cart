using Backend.Models;

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
        public bool isSoldOut { get; set; } = false;
    }
}
