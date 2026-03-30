using Backend.Data;
using Backend.EndPoints.ShoppingCart.Cart.DTO;
using Backend.Models.Foods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.EndPoints.ShoppingCart.Items;


[Route("itemAdmin")]
[ApiController]
public class AdminItem : ControllerBase
{
    private readonly AppDbContext _cartContext;
    public AdminItem(AppDbContext cartContext)
    {
        _cartContext = cartContext;
    }

    [HttpPost("Create"), Authorize(Roles = "Admin")]
    public async Task<ActionResult> InsertItemsToMenu
        ([FromBody] List<AdminCartDTO.AdminCreateFoodDto> items)
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
                Carbs = item.Carbs
            }
        }).ToList();

        await _cartContext.Foods.AddRangeAsync(foods);
        await _cartContext.SaveChangesAsync();

        return Ok("Items were successfully inserted into the menu.");
    }
    [HttpGet("Read")]
    public async Task<ActionResult> ReadItemFromMenu()
    {
        var foods = await _cartContext.Foods.ToListAsync();
        return Ok(foods);
    }
    //Edit the item in menu with provided name of the item.
    [HttpPut("update/{Name}"), Authorize(Roles = "Admin")]
    public async Task<ActionResult> ModifyItemByName
        (string Name, [FromBody] AdminCartDTO.AdminCreateFoodDto newItem)
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
        olditem.Nutrition = new Nutrition
        {
            Calories = newItem.Calories,
            Protein = newItem.Protein,
            Carbs = newItem.Carbs
        };
        olditem.Description = newItem.Description;
        olditem.FoodType = newItem.FoodType;
        await _cartContext.SaveChangesAsync();
        return Ok();
    }
    [HttpDelete("delete/{Name}"), Authorize(Roles ="Admin")]
    public async Task<ActionResult> DeleteItem(string Name)
    {
        var delete = await _cartContext.Foods.FirstOrDefaultAsync(x => x.Name == Name);
        if (delete == null) return NotFound("Sorry, we cannot find the item in our database.");
        _cartContext.Foods.Remove(delete);
        await _cartContext.SaveChangesAsync();
        return Ok("Item deleted successfully!");
    }
}
