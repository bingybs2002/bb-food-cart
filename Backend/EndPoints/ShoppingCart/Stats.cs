using Backend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            .Where(c => c.IsCheckedOut && c.CreatedDate==Today)
            .GroupBy(c => c.CreatedDate)
            .Select(g => new { Day = g.Key, Transactions = g.Count() })
            .OrderBy(r => r.Day)
            .ToListAsync();
        
        return Ok(salesToday.Count());
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
        return Ok(salesLast7days.Count());
    }
}
