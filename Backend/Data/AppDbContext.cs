using Backend.Models.Account;
using Backend.Models.Cart;
using Backend.Models.Foods;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public class AppDbContext : IdentityDbContext<IdentityUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<UserAccount> Customers => Set<UserAccount>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Food> Foods => Set<Food>();
    public DbSet<ShoppingCart> ShoppingCarts => Set<ShoppingCart>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //Declare that the required namefields are required for userAccount
        modelBuilder.Entity<UserAccount>(entity =>
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


        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(refreshToken => refreshToken.Id);
            entity.Property(refreshToken => refreshToken.Token).IsRequired();

            entity.HasOne(refreshToken => refreshToken.User)
                  .WithMany()//placeholder? 
                  .HasForeignKey(refreshToken => refreshToken.UserId)
                  .IsRequired();
        });

        //shopping cart (1 to many) items
        modelBuilder.Entity<ShoppingCart>()
             .HasMany(s => s.Items)
             .WithOne(s => s.ShoppingCart)
             .HasForeignKey(e => e.ShoppingCartId);

        //shopping cart (1 to 1) customer
        modelBuilder.Entity<UserAccount>()
            .HasOne(s => s.ShoppingCart)
            .WithOne(s => s.Customer)
            .HasForeignKey<ShoppingCart>(s => s.CustomerId);
    }
}
