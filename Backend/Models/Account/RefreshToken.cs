using Microsoft.AspNetCore.Identity;

namespace Backend.Models.Account
{
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
}
