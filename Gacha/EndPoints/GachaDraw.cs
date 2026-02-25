using Gacha.Data;
using System.Linq;
using Gacha.Models;
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
                var count = await db.Items.CountAsync();
                Console.Out.WriteLine("Numaber of Counts: " + count);

                var index = Random.Shared.Next(1,count+1);
                var result = db.Items.FirstOrDefault(x=>x.Id == index);
                Console.Out.WriteLine("Random Generated: " + index);
                return Results.Ok(result);
            });
        }
    }
}
