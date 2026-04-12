using Backend.Data;
using System.Text.Json;
using Backend.EndPoints.ShoppingCart;
namespace Backend.Testing.Menu
{
    public class SeedMenu
    {
        public static async Task SeedMenuItems(AppDbContext db)
        {
            if (db.Foods.Any())
            {
                return;
            }
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var json = await File.ReadAllTextAsync("Testing/Menu/menu.json");
            var items = JsonSerializer.Deserialize<List<Backend.EndPoints.ShoppingCart.Cart.DTO.AdminCartDTO.AdminCreateFoodDto>>(json, options);

            if (items == null) {
                Console.WriteLine("Menu item already serialized.");
                return; 
            }

            var httpclient = new HttpClient();

 
            var response = await httpclient.PostAsJsonAsync("https://localhost:63196/itemAdmin/Create-SEED", items);

              
            

        }
    }
}
