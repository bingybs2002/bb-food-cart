using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Backend.Models.Account;
using Backend.Models.Cart;
using Backend.Models.Food;

namespace Backend.Data;

public class AppDbContext : IdentityDbContext<IdentityUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<AccountUser> Customers => Set<AccountUser>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    //public DbSet<Food> Foods => Set<Food>();
    public DbSet<ShoppingCart> Carts => Set<ShoppingCart>();
    public DbSet<ShoppingCartItem> CartItems => Set<ShoppingCartItem>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("auth");

        builder.Entity<AccountUser>(entity =>
        {
            entity.HasKey(customer => customer.Id);
            entity.Property(customer => customer.Cosignee).IsRequired();
            entity.Property(customer => customer.StreetAddress).IsRequired();
            entity.Property(customer => customer.City).IsRequired();
            entity.Property(customer => customer.State).IsRequired();
            entity.Property(customer => customer.ZipCode).IsRequired();

            entity.HasOne(customer => customer.User)
                  .WithMany()
                  .HasForeignKey(customer => customer.UserId)
                  .IsRequired();
        });

        builder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(refreshToken => refreshToken.Id);
            entity.Property(refreshToken => refreshToken.Token).IsRequired();
            entity.HasIndex(refreshToken => refreshToken.Token).IsUnique();

            entity.HasOne(refreshToken => refreshToken.User)
                  .WithMany()
                  .HasForeignKey(refreshToken => refreshToken.UserId)
                  .IsRequired();
        });

        builder.Entity<ShoppingCart>(entity =>
        {
            entity.HasKey(cart => cart.Id);

            entity.HasOne(cart => cart.Customer)
                  .WithOne(customer => customer.ShoppingCart)
                  .HasForeignKey<ShoppingCart>(cart => cart.CustomerId);

            entity.HasIndex(cart => cart.CustomerId).IsUnique();
        });
    }
}
