using Backend.Data;
using static Backend.Models.Gacha.GachaModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Backend.EndPoints.Gacha
{
    public static class Gacha
    {
        public static void GachaLuckyPicker(this IEndpointRouteBuilder r)
        {
            var groups = r.MapGroup("Gacha").WithTags("Gacha");

            //POST bulk insert 
            groups.MapPost("insert", async (AppDbContext db, List<GachaItemType> items) =>
            {
                await db.GachaItems.AddRangeAsync(items);
                await db.SaveChangesAsync();

                return Results.Created("/bulk", items);
            }).RequireAuthorization("Admin");

            groups.MapDelete("delete/{id}", async (int id, AppDbContext db) =>
            {
                var item = await db.GachaItems.FindAsync(id);
                if (item is null)
                {
                    return Results.NotFound($"Item with id {id} not found.");
                }
                db.GachaItems.Remove(item);
                await db.SaveChangesAsync();
                return Results.Ok($"Item.with id {id} deleted");
            }).RequireAuthorization("Admin");

            //DELETE delete all
            groups.MapDelete("delete-all", async (AppDbContext db) =>
            {
                await db.GachaItems.ExecuteDeleteAsync();
                return Results.NoContent();
            }).RequireAuthorization("Admin");
            groups.MapGet("TESTING-pick", async (AppDbContext db) =>
            {
                var count = await db.GachaItems.CountAsync();
                Console.Out.WriteLine("Numaber of Counts: " + count);

                var index = Random.Shared.Next(1, count + 1);
                var result = db.GachaItems.FirstOrDefault(x => x.Id == index);
                Console.Out.WriteLine("Random Generated: " + index);
                return Results.Ok(result);
            });
            groups.MapGet("pick", async (AppDbContext db) =>
            {
                var count = await db.GachaItems.CountAsync();
                Console.Out.WriteLine("Numaber of Counts: " + count);

                var index = Random.Shared.Next(1, count + 1);
                var result = db.GachaItems.FirstOrDefault(x => x.Id == index);
                Console.Out.WriteLine("Random Generated: " + index);
                return Results.Ok(result);
            }).RequireAuthorization("User");
            groups.MapGet("all-items", async (AppDbContext db) =>
                await db.GachaItems.ToListAsync());
        }
    }
}
