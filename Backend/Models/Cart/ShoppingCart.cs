using Backend.Models.Account;
namespace Backend.Models.Cart
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public UserAccount Customer { get; set; } = null!;//one to one
        public bool IsCheckedOut { get; set; } = false;
        public bool IsCancelled { get; set; } = false;
        public List<CartItem> Items { get; set; } = [];
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
