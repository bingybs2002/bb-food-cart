using Backend.Models;
using Backend.Models.Foods;

namespace Backend.EndPoints.Cart;

public class ShoppingCartDTO
{
    public class ShoppingCartResponseDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public List<FoodDto> Items { get; set; } = new();
    }
    public class FoodDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
    }
    public class CreateFoodDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Allergies Allergies { get; set; } = Allergies.None;
        public FoodType FoodType { get; set; }
        public bool IsSoldOut { get; set; } = false;
    }
}
