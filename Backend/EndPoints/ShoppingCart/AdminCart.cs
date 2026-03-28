using Backend.Data;
using Backend.Models.Foods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.EndPoints.Cart;

[Route("cartAdmin")]
[ApiController]
public class AdminCart : ControllerBase
{
    private readonly AppDbContext _cartContext;
    public AdminCart(AppDbContext cartContext)
    {
        _cartContext = cartContext; 
    }
    //Find the shopping cart by customer phone number.
    [HttpGet("find/{phoneNumber}"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<AdminCartDTO.AdminShoppingCartDto>> GetCustomerCartByPhone(string phoneNumber)
    {
        var normalizedPhone = new string(phoneNumber.Where(char.IsDigit).ToArray());

        var cart = await _cartContext.ShoppingCarts
            .Include(cart => cart.Customer)
            .ThenInclude(customer => customer.User)
            .Include(cart => cart.Items)
            .ThenInclude(item => item.Food)
            .FirstOrDefaultAsync(cart =>
                cart.Customer.User != null &&
                cart.Customer.User.PhoneNumber != null &&
                new string(cart.Customer.User.PhoneNumber.Where(char.IsDigit).ToArray()) == normalizedPhone);

        if (cart == null)
        {
            return Ok("No cart was found, or the phone number is invalid");
        }

        var response = new AdminCartDTO.AdminShoppingCartDto
        {
            Id = cart.Id,
            CustomerId = cart.CustomerId,
            Foods = cart.Items.Select(f => new AdminCartDTO.AdminFoodDto
            {
                Id = f.Food.Id,
                Name = f.Food.Name
            }).ToList()
        };

        return Ok(response);
    }
    //Registered admin can edit the item in menu
    [HttpPut("edit/{Name}"), Authorize(Roles = "Admin")]
    public async Task<ActionResult> ModifyItemByName(string Name, [FromBody]Food newItem)
    {
        var olditem = await _cartContext.Foods.
            FirstOrDefaultAsync(x => x.Name == Name);
        if (olditem == null)
        {
            return NotFound($"No item matches the name provided.");
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
    [HttpPost("insert"), Authorize(Roles = "Admin")]
    public async Task<ActionResult> InsertItemsToMenu([FromBody] List<AdminCartDTO.AdminCreateFoodDto> items)
    {
        if (items == null || items.Count == 0)
            return BadRequest("No items were provided.");

        var foods = items.Select(item => new Food
        {
            Name = item.Name,
            Description = item.Description,
            Allergies = item.Allergies,
            FoodType = item.FoodType,
            IsSoldOut = item.IsSoldOut,
            Nutrition = new Nutrition
            {
                Calories = item.Calories,
                Protein = item.Protein,
                Carbs = item.Carb
            }
        }).ToList();

        await _cartContext.Foods.AddRangeAsync(foods);
        await _cartContext.SaveChangesAsync();

        return Ok("Items were successfully inserted into the menu.");
    }
}
