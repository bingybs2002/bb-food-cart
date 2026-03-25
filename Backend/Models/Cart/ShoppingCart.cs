using Backend.Models.Account;
namespace Backend.Models.Cart
{
    public class ShoppingCart
    {
        public int Id { get; set; }//PK
        public int CustomerId { get; set; }//Fk -> AccountUser.Id
        public AccountUser Customer { get; set; } = default!;//Navigation property
        public bool IsCheckedOut { get; set; }
        public DateTime CreatedAt{ get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<ShoppingCartItem> Items { get; set; } = new();//Navigation property, one to many
    }
}
