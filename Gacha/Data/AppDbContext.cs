using Gacha.Models;
using Microsoft.EntityFrameworkCore;

namespace Gacha.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        //enable query and migrations
        public DbSet<ItemType> Items { get; set; } = null!;

    }
}
