using Microsoft.AspNetCore.Identity;

namespace Backend.Models.Food;

public class Customer
{
    public int Id { get; set; }
    public string UserId { get; set; } = default!;
    public IdentityUser User { get; set; } = default!;
    public required string Cosignee { get; set; }
    public Backend.Models.Food.Allergies Allergies { get; set; }
    public required string StreetAddress { get; set; }
    public required string City { get; set; }
    public required string State { get; set; }
    public required string ZipCode { get; set; }
}