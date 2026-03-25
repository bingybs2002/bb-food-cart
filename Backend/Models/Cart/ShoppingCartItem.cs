namespace Backend.Models.Cart
{
    public class ShoppingCartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int FoodId { get; set; }
        public int Quantity { get; set; }
    }
}
