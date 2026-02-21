using Gacha.Data;
using Gacha.Models;

namespace Gacha.EndPoints
{
    public static class GachaCreate
    {
        public static void GachaCreateItem(this IEndpointRouteBuilder r)
        {
            var groups = r.MapGroup("Gacha");

            /// POST create item
            groups.MapPost(string.Empty, async (AppDbContext db, ItemType items) =>
            {
                db.Items.Add(items);
                await db.SaveChangesAsync();

                return Results.Created($"gacha/{items.Id}", items);
            });

            //POST bulk insert 
            groups.MapPost("/bulk", async (AppDbContext db, List<ItemType> items) =>
            {
                await db.Items.AddRangeAsync(items);
                await db.SaveChangesAsync();

                return Results.Created($"gacha/bulk", items);
            });

        }
    }
}
