using Backend.Models.Food;
using Backend.Models.Cart;

using Microsoft.AspNetCore.Identity;

namespace Backend.Models.Account;

public class UserAccount
{
    public int Id { get; set; }//Pk
    public string? UserId { get; set; }//Fk -> IdentityUser.Id
    public IdentityUser? User { get; set; }//Navigation -> Identity User Auth
    public required string Cosignee { get; set; }
    public Allergies Allergies { get; set; }
    public required string StreetAddress { get; set; }
    public required string City { get; set; }
    public required string State { get; set; }
    public required string ZipCode { get; set; }
    public ShoppingCart? ShoppingCart { get; set; }//Navigation -> Cart
}

public class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; } = default!;
    public string UserId { get; set; } = default!;
    public IdentityUser User { get; set; } = default!;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime ExpiresAtUtc { get; set; }
    public DateTime? RevokedAtUtc { get; set; }
    public bool IsActive => RevokedAtUtc is null && ExpiresAtUtc > DateTime.UtcNow;
}
