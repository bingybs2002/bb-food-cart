using Backend.Data;
using Backend.EndPoints.ShoppingCart.Cart.DTO;
using Backend.Models.Account;
using Backend.Models.Foods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Backend.EndPoints.ShoppingCart.Cart;

[Route("cartAdmin")]
[ApiController]
public class AdminCart : ControllerBase
{
    private readonly AppDbContext _cartContext;
    public AdminCart(AppDbContext cartContext)
    {
        _cartContext = cartContext; 
    }

    //returns the current user
    private async Task<UserAccount?> GetUserAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return null;
        }
        var response = await _cartContext.Customers
            .FirstOrDefaultAsync(c => c.UserId == userId);
        Console.WriteLine("User found: ", response);
        return await _cartContext.Customers
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }

    private async Task<Models.Cart.ShoppingCart?> GetLatestCartByUser()
    {
        var customer = await GetUserAsync();
        if (customer == null)
        {
            Console.WriteLine("Customer Not Found!");
            return null;
        }
        return await _cartContext.ShoppingCarts
                .Include(c => c.Items)
                .ThenInclude(i => i.Food)
                .OrderBy(c => c.CreatedDate)
                .LastOrDefaultAsync(c => c.CustomerId == customer.Id);
    }
    //Find the shopping cart by phone number
    
    [HttpGet("CustomerShoppingCartHistory/{phoneNumber}"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<AdminCartDTO.AdminShoppingCartDto>>>
    GetCustomerShoppingCartHistory(string phoneNumber)
    {
        var customer = await _cartContext.Customers
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.User != null && c.User.PhoneNumber == phoneNumber);

        if (customer == null)
        {
            return NotFound("Customer not found.");
        }

        var carts = await _cartContext.ShoppingCarts
            .Include(c => c.Items)
            .ThenInclude(i => i.Food)
            .Where(c => c.CustomerId == customer.Id)
            .OrderBy(c => c.CreatedDate)
            .ToListAsync();

        var response = carts.Select(cart => new AdminCartDTO.AdminShoppingCartDto
        {
            Id = cart.Id,
            CustomerId = cart.CustomerId,
            Foods = cart.Items.Select(item => new AdminCartDTO.AdminFoodDto
            {
                Id = item.Food.Id,
                Name = item.Food.Name
            }).ToList(),
            CreatedAtUtc = cart.CreatedDate
        }).ToList();

        return Ok(response);
    }
    [HttpGet("OrderHistory")]
    public async Task<ActionResult> OrderHistory()
    {
        var ret = await _cartContext.ShoppingCarts
            .Include(c => c.Items)
            .ThenInclude(i => i.Food)
            .OrderByDescending(c => c.CreatedDate)
            .Select(c => new
            {
                c.Id,
                c.CustomerId,
                c.IsCheckedOut,
                c.CreatedDate,
                c.IsCancelled,
                Items = c.Items.Select(i => new
                {
                    i.Id,
                    i.Quantity,
                    FoodName = i.Food.Name
                }).ToList()
            })
            .ToListAsync();

        return Ok(ret);
    }


}
