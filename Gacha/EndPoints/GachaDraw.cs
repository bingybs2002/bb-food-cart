using Gacha.Data;
using Microsoft.EntityFrameworkCore;

namespace Gacha.EndPoints
{
    public static class GachaDraw
    {
        public static void GachaDrawItem(this IEndpointRouteBuilder r)
        {
            var group = r.MapGroup("gacha/draw");
            //Get by drawing
            group.MapGet("/", async (AppDbContext db) =>
            {
                var Count = await db.Items.CountAsync();
                Console.Out.WriteLine("Numaber of Counts: " + Count);

                var Index = Random.Shared.Next(1,Count+1);
                Console.Out.WriteLine("Random Generated: " + Index);
                return Results.Ok(Index);
            });
        }
    }
}
