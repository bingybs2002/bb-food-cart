using Gacha.Data;
using Microsoft.EntityFrameworkCore;
namespace Gacha.EndPoints
{
    public static class GachaDelete
    {
        public static void GachaDeleteItem(this IEndpointRouteBuilder r)
        {
            var group = r.MapGroup("gacha");
            //DELETE delete by id
            group.MapDelete("/{id}", async (int id, AppDbContext db) =>
            {
                var item = await db.Items.FindAsync(id);
                if (item is null)
                {
                    return Results.NotFound($"Item with id {id} not found.");
                }
                db.Items.Remove(item);
                await db.SaveChangesAsync();
                return Results.Ok($"Item.with id {id} deleted");
            });

            //DELETE delete all
            group.MapDelete("/reset", async (AppDbContext db) =>
            {
                await db.Items.ExecuteDeleteAsync();
                return Results.NoContent();
            });

        }
    }
}
