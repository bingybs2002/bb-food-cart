using Backend.Data;
using Backend.Models.Cart;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Backend.EndPoints.Cart;

[Route("Cart")]
[ApiController]
public class ShoppingCart : ControllerBase
{
    private readonly AppDbContext _cartContext;
    public ShoppingCart(AppDbContext cartContext)
    {
        _cartContext = cartContext;
    }
    [HttpGet("myCart"), Authorize(Roles = "User")]
    //Get shopping cart by the logged in customer. If no shopping cart is found,
    //create a new one for the user.
    public async Task<ActionResult<ShoppingCartDTO.ShoppingCartResponseDto>> GetShoppingCartByLoggedInCustomer()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User not found by UserId at GetShoppingCartByLoggedInCustomer");
        }
        var customer = await _cartContext.Customers.
            FirstOrDefaultAsync(c => c.UserId == userId);

        if (customer == null)
        {
            return NotFound("Customer Account is not found");
        }
        var cart = await _cartContext.ShoppingCarts
                .Include(c => c.Items)
                .ThenInclude(i=>i.Food)
                .FirstOrDefaultAsync(c => c.CustomerId == customer.Id);
        if (cart == null)
        {
            cart = new Models.Cart.ShoppingCart
            {
                CustomerId = customer.Id
            };

            _cartContext.ShoppingCarts.Add(cart);
            await _cartContext.SaveChangesAsync();
        }
        var response = new ShoppingCartDTO.ShoppingCartResponseDto
        {
            Id = cart.Id,
            CustomerId = cart.CustomerId,
            Items = cart.Items.Select(f => new ShoppingCartDTO.FoodDto
            {
                Id = f.Food.Id,
                Name = f.Food.Name
            }).ToList()
        };
        return Ok(response);
    }
    [HttpPost("AddToCart"),Authorize(Roles ="User")]
    public async Task<ActionResult>
}
