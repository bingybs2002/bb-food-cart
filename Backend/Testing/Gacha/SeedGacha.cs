using Backend.Data;
using System.Text.Json;
using Backend.Models.Gacha;
using Backend.EndPoints.Gacha;

namespace Backend.Testing.Gacha
{
    public static class SeedGacha
    {
        public static async Task SeedAsync(AppDbContext db)
        {
            if(db.GachaItems.Any())
            {
                return;
            }

            var json = await File.ReadAllTextAsync("Testing/Gacha/GachaItems.json");
            var items = JsonSerializer.Deserialize<List<GachaModel.GachaItemType>>(json);

            if(items != null)
            {
                db.GachaItems.AddRange(items);
                await db.SaveChangesAsync();
            }   
        }
    }
}
