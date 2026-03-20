using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Backend.Models.Account;

namespace Backend.Data;

public class AuthDbContext : IdentityDbContext<IdentityUser>
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("auth");

        builder.Entity<Customer>(entity =>
        {
            entity.HasKey(customer => customer.Id);
            entity.Property(customer => customer.Cosignee).IsRequired();
            entity.Property(customer => customer.StreetAddress).IsRequired();
            entity.Property(customer => customer.City).IsRequired();
            entity.Property(customer => customer.State).IsRequired();
            entity.Property(customer => customer.ZipCode).IsRequired();
            entity.HasOne(customer => customer.User).WithMany().HasForeignKey(customer => customer.UserId).IsRequired();
        });

        builder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(refreshToken => refreshToken.Id);
            entity.Property(refreshToken => refreshToken.Token).IsRequired();
            entity.HasIndex(refreshToken => refreshToken.Token).IsUnique();
            entity.HasOne(refreshToken => refreshToken.User).WithMany().HasForeignKey(refreshToken => refreshToken.UserId).IsRequired();
        });
    }
}
