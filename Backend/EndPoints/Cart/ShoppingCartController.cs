using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models.Cart;
using Microsoft.AspNetCore.Authorization;
using Backend.Models.Food;
using System.Security.Claims;
using Backend.Migrations;
using Backend.Models;

namespace Backend.EndPoints.Cart;


[Route("Cart/[controller]")]
[ApiController]
public class ShoppingCartController : ControllerBase
{
    public class ShoppingCartResponseDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public List<FoodDto> Foods { get; set; } = new();
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

    private readonly AppDbContext _cartContext;
    public ShoppingCartController(AppDbContext cartContext)
    {
        _cartContext = cartContext;
    }

    //Find the shopping cart by customer id. If no, create one.
    [HttpGet("Admin/Cart/{phoneNumber}"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<ShoppingCart>> GetShoppingCartByCustomerPhone(string phoneNumber)
    {
        var normalizedPhone = new string(phoneNumber.Where(char.IsDigit).ToArray());
    var cart = await _cartContext.ShoppingCarts
        .Include(cart => cart.Foods)
        .Include(cart => cart.Customer)
        .ThenInclude(customer => customer.User)
        .FirstOrDefaultAsync(cart =>
            cart.Customer.User != null &&
            cart.Customer.User.PhoneNumber == normalizedPhone);
        if (cart == null)
        {
            return Ok("No cart has found");
        }
        var response = new ShoppingCartResponseDto
        {
            Id = cart.Id,
            CustomerId = cart.CustomerId,
            Foods = cart.Foods.Select(f => new FoodDto
            {
                Id = f.Id,
                Name = f.Name
            }).ToList()
        };
        return Ok(response);
    }

    [HttpGet("my/cart"), Authorize(Roles = "User")]
    public async Task<ActionResult<ShoppingCart>> GetShoppingCartByLoggedInCustomer()
    {
       var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User cannot be found");
        }
        var customer = await _cartContext.Customers.
            FirstOrDefaultAsync(c => c.UserId == userId);

        if (customer == null)
        {
            return NotFound("Customer Account is not found");
        }
        var cart = await _cartContext.ShoppingCarts
                .Include(c => c.Foods)
                .FirstOrDefaultAsync(c => c.CustomerId == customer.Id);

        if (cart == null)
        {
            cart = new ShoppingCart
            {
                CustomerId = customer.Id
            };

            _cartContext.ShoppingCarts.Add(cart);
            await _cartContext.SaveChangesAsync();  
        }
        var response = new ShoppingCartResponseDto
        {
            Id = cart.Id,
            CustomerId = cart.CustomerId,
            Foods = cart.Foods.Select(f => new FoodDto
            {
                Id = f.Id,
                Name = f.Name
            }).ToList()
        };
        return Ok(response);
    }

    [HttpPost("cart/insert")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> InsertItemsToMenu([FromBody] List<CreateFoodDto> items)
    {
        if (items == null || items.Count == 0)
            return BadRequest("No items were provided.");

        var foods = items.Select(item => new Food
        {
            Name = item.Name,
            Description = item.Description,
            Allergies = item.Allergies,
            FoodType = item.FoodType,
            IsSoldOut = item.IsSoldOut
        }).ToList();

        await _cartContext.Foods.AddRangeAsync(foods);
        await _cartContext.SaveChangesAsync();

        return Ok("Items were successfully inserted into the menu.");
    }

    //Registered admin can edit the item in menu
    [HttpPut, Authorize(Roles = "Admin")]
    public  async Task<ActionResult> ModifyItemByName(string Name, Food newItem)
    {
        var olditem = await _cartContext.Foods.
            FirstOrDefaultAsync(x=>x.Name == Name);
        if(olditem == null)
        {
            return BadRequest($"No item matches the name provided.");
        }
        olditem.Name = newItem.Name;
        olditem.Allergies = newItem.Allergies;
        olditem.IsSoldOut = newItem.IsSoldOut;
        olditem.Nutrition = newItem.Nutrition;
        olditem.Description = newItem.Description;
        olditem.FoodType = newItem.FoodType;
        await _cartContext.SaveChangesAsync();
        return Ok();
    }
}
