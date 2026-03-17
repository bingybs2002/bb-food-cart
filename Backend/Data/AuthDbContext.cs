using Backend.Models.Food;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Backend.Models.Account;
namespace Backend.Data
{
    public class AuthDbContext : IdentityDbContext<IdentityUser>
    {
        public AuthDbContext(DbContextOptions options) : base(options){}

        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        //Overwrite auth to store under "auth" schema
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("auth");
            builder.Entity<Customer>(entity =>
            {
                entity.HasKey(Customer => Customer.Id);

                entity.Property(Customer => Customer.Cosignee).IsRequired();
                entity.Property(Customer => Customer.StreetAddress).IsRequired();
                entity.Property(Customer => Customer.City).IsRequired();
                entity.Property(Customer => Customer.State).IsRequired();
                entity.Property(Customer => Customer.ZipCode).IsRequired();

                entity.HasOne(Customer => Customer.User).WithMany().
                    HasForeignKey(Customer => Customer.UserId).IsRequired();
            });

            builder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(refreshToken => refreshToken.Id);

                entity.Property(refreshToken => refreshToken.Token).IsRequired();
                entity.HasIndex(refreshToken => refreshToken.Token).IsUnique();
                entity.HasOne(RefreshToken => RefreshToken.User).WithMany().
                        HasForeignKey(RefreshToken => RefreshToken.UserId).IsRequired();
            });

        }

    }
}
