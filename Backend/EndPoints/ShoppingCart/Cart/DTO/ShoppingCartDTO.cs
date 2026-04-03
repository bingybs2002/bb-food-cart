using Backend.Models;
using Backend.Models.Foods;
using static Backend.EndPoints.ShoppingCart.Cart.DTO.AdminCartDTO;

namespace Backend.EndPoints.ShoppingCart.Cart.DTO;

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
        public int Quantity { get; set; }
    }
    public class CreateFoodDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Allergies Allergies { get; set; } = Allergies.None;
        public FoodType FoodType { get; set; }
        public bool IsSoldOut { get; set; } = false;
    }
    public class CartItemDto
    {
        public required string FoodName { get; set; }
        public int Quantity { get; set; }
    }
    public class ChangeItemQuantityDTO
    {
        public required string FoodName { get; set; }
        public int Quantity { get; set; }
    }
    public class DeleteItemDTO
    {
        public required string FoodName { get; set; }
    }
}
