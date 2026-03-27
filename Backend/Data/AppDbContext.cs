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

    public DbSet<UserAccount> Customers => Set<UserAccount>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Food> Foods => Set<Food>();
    public DbSet<ShoppingCart> ShoppingCarts => Set<ShoppingCart>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<UserAccount>(entity =>
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

            entity.HasOne(refreshToken => refreshToken.User)
                  .WithMany()//placeholder? 
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

            entity.HasMany(cart => cart.Foods).
                WithOne(cart => cart.Cart).
                HasForeignKey(cart => cart.CartId);
        });
    }
}
