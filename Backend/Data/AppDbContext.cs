using Backend.Models.Account;
using Backend.Models.Cart;
using Backend.Models.Foods;
using Backend.Models.Gacha;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using static Backend.Models.Gacha.GachaModel;

namespace Backend.Data;

public class AppDbContext : IdentityDbContext<IdentityUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<UserAccount> Customers => Set<UserAccount>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Food> Foods => Set<Food>();
    public DbSet<ShoppingCart> ShoppingCarts => Set<ShoppingCart>();
    public DbSet<GachaItemType> GachaItems { get; set; } = null!;
    public DbSet<CartItem> CartItems => Set<CartItem>();

    public DbSet<Nutrition> Nutrition => Set<Nutrition>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //Declare that the required namefields are required for userAccount
        modelBuilder.Entity<UserAccount>(entity =>
        {
            entity.HasKey(customer => customer.Id);
            entity.Property(customer => customer.UserName).IsRequired();
            entity.Property(customer => customer.StreetAddress).IsRequired();
            entity.Property(customer => customer.City).IsRequired();
            entity.Property(customer => customer.State).IsRequired();
            entity.Property(customer => customer.ZipCode).IsRequired();

            entity.HasOne(customer => customer.User)
                  .WithMany()
                  .HasForeignKey(customer => customer.UserId)
                  .IsRequired();
        });


        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(refreshToken => refreshToken.Id);
            entity.Property(refreshToken => refreshToken.Token).IsRequired();

            entity.HasOne(refreshToken => refreshToken.User)
                  .WithMany()//placeholder? 
                  .HasForeignKey(refreshToken => refreshToken.UserId)
                  .IsRequired();
        });


        //shopping cart (one to many) items
        modelBuilder.Entity<ShoppingCart>()
             .HasMany(s => s.Items)
             .WithOne(s => s.ShoppingCart)
             .HasForeignKey(e => e.ShoppingCartId);

        //shopping cart (many to 1) customer
        modelBuilder.Entity<UserAccount>()
            .HasMany(s => s.ShoppingCart)
           .WithOne(s=>s.Customer)
           .HasPrincipalKey(s=>s.Id)
           .HasForeignKey(s=>s.CustomerId);

    }
}
