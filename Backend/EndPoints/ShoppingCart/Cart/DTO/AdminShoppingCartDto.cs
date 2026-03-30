using Backend.Models;
using Backend.Models.Foods;

namespace Backend.EndPoints.ShoppingCart.Cart.DTO;

public class AdminCartDTO
{
    public class AdminShoppingCartDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public List<AdminFoodDto> Foods { get; set; } = new();
    }
    public class AdminFoodDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
    }
    public class AdminCreateFoodDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Allergies Allergies { get; set; } = Allergies.None;
        public FoodType FoodType { get; set; }
        public bool IsSoldOut { get; set; } = false;
        public int Calories { get; set; }
        public int Protein { get; set; }
        public int Carbs { get; set; }
    }
}
