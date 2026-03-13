using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;

namespace Backend.Models.Account;

public class Authentication
{
    public int Id { get; set; }
    public string Token { get; set; } = default!;
    public string UserId { get; set; } = default!;
    public IdentityUser User { get; set; } = default!;
    public DateTime CreatedAtUTC { get; set; }
    public DateTime ExpiresAtUTC { get; set; }
    public DateTime? RevokedAtUTC { get; set; }
    public bool IsActive => RevokedAtUTC is null && ExpiresAtUTC > DateTime.UtcNow;
}