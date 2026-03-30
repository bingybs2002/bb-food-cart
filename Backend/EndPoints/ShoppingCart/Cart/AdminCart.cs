using Backend.Data;
using Backend.EndPoints.ShoppingCart.Cart.DTO;
using Backend.Models.Foods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    //Find the shopping cart by customer phone number,
    //return the items in list.
    [HttpGet("{phoneNumber}"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<AdminCartDTO.AdminShoppingCartDto>> 
        GetCustomerCartByPhone(string phoneNumber)
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
                new string(cart.Customer.User.PhoneNumber.Where(char.IsDigit).
                    ToArray()) == normalizedPhone);
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



}
