using System.Runtime.CompilerServices;
using Backend.Models;
using Backend.Models.Cart;
using Backend.Data;
using System.ComponentModel.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
namespace Backend.EndPoints.Cart
{
    public static class CartEndpoint
    {
        public static void MapCartEndpoints(this IEndpointRouteBuilder routes)
        {
            var a = routes.MapGroup("Cart");
            a.MapPost("GetCartIdByCustomerId", async (ShoppingCartItem cart, AppDbContext db, UserManager<IdentityUser> userManager) =>
            {
                var Carto = await db.Carts.SingleOrDefaultAsync();
                 
            }).RequireAuthorization();
        }
    }
}
