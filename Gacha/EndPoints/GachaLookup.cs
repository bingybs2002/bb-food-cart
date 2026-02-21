using Gacha.Data;
using Microsoft.EntityFrameworkCore;

namespace Gacha.EndPoints
{
    public static class GachaLookup
    {
        public static void GachaItemLookup(this IEndpointRouteBuilder r)
        {
            var group = r.MapGroup("/");            
            group.MapGet("", () => "Homepage");

            //GET Getting all items
           group.MapGet("gacha", async (AppDbContext db) => 
                await db.Items.ToListAsync());
        }
    }
}