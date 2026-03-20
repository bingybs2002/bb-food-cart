using Backend.Models.Food;
using Microsoft.AspNetCore.Identity;

namespace Backend.Models.Account;

public class Customer
{
    public int Id { get; set; }
    public string UserId { get; set; } = default!;
    public IdentityUser User { get; set; } = default!;
    public required string Cosignee { get; set; }
    public Allergies Allergies { get; set; }
    public required string StreetAddress { get; set; }
    public required string City { get; set; }
    public required string State { get; set; }
    public required string ZipCode { get; set; }
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
