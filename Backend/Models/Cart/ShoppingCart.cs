using Backend.Models.Account;
using Backend.Models.Food;
namespace Backend.Models.Cart
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public UserAccount Customer { get; set; } = null!;//one to one
        public bool IsCheckedOut { get; set; }
        public List<Food.Food> Foods { get; set; } = new List<Food.Food>();//one to many
    }
}
