using Backend.Data;
using Backend.Models.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
namespace Backend.EndPoints.ShoppingCart;

[Route("Stat")]
[ApiController]
public class Stats : ControllerBase
{
    private readonly AppDbContext _statContext;
    public Stats(AppDbContext statContext)
    {
        _statContext = statContext;
    }

    //[HttpGet("salesToday"),Authorize(Roles ="Admin")]
    [HttpGet("salesToday")]
    public async Task<ActionResult> SalesToday()
    {
        var Today = DateTime.UtcNow;
        var salesToday = await _statContext.ShoppingCarts
            .Where(c => c.IsCheckedOut && c.CreatedDate>Today.AddDays(-1))
            .GroupBy(c => c.CreatedDate)
            .Select(g => new { Day = g.Key, Transactions = g.Count() })
            .OrderBy(r => r.Day)
            .ToListAsync();
        var sales = salesToday.Sum(x => x.Transactions); 
        return Ok(sales);
    }

    //[HttpGet("salesToday"),Authorize(Roles ="Admin")]
    [HttpGet("salesInLast7Days")]
    public async Task<ActionResult> SalesPerWeek()
    {
        var Today = DateTime.UtcNow;
        var StartDate = DateTime.UtcNow.AddDays(-7);

        var salesLast7days = await _statContext.ShoppingCarts
            .Where(c => c.IsCheckedOut &&
                    c.CreatedDate.Date >= StartDate &&
                    c.CreatedDate.Date < Today.AddDays(1))
            .GroupBy(c => c.CreatedDate.Date)
            .Select(g => new
            {
                Day = g.Key,
                Transactions = g.Count()
            })
            .OrderBy(x => x.Day)
            .ToListAsync();

        var sales = salesLast7days.Sum(x => x.Transactions);
        return Ok(sales);
    }

    [HttpGet("MostPopularItem")]
    public async Task<ActionResult> MostPopularItem()
    {
        var PopularItem = await _statContext.CartItems
            .GroupBy(c => c.FoodId)
            .Select(g => new
            {
                FoodId = g.Key,
                TotalSold = g.Sum(x => x.Quantity)
            })
            .OrderByDescending(x => x.TotalSold)
            .FirstOrDefaultAsync();
        if (PopularItem == null) return BadRequest("Statistics are not high enough for this.");

        var food = await _statContext.Foods
            .FirstOrDefaultAsync(f => f.Id == PopularItem.FoodId);
        if (food==null) { return BadRequest("Cannot find the food tag"); }
        var nutrition = await _statContext.Nutrition
            .FirstOrDefaultAsync(f => f.Id == food.Id);
        if (nutrition == null) { return BadRequest("Cannot find the nutrition tag."); }
        var ret = new
        {
            food.Name,
            nutrition.Calories,
            nutrition.Carbs,
            nutrition.Protein,
            PopularItem.TotalSold
        };
        return Ok(ret);
    }

    [HttpGet("SoldOutItems")]
    public async Task<ActionResult> SoldOutItems()
    {
        var foods = await _statContext.Foods
            .Where(f => f.IsSoldOut == true)
            .Select(g=> new
            {
                g.Id,
                g.Name,
                g.Allergies,
                g.Description,
                g.Nutrition,
            })
            .ToListAsync();
        return Ok(foods);
    }
}
