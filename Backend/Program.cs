using Backend.Data;
using Backend.EndPoints.Admin;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Backend.EndPoints.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("Connection")));

builder.Services.AddAuthorization();

// Identity
builder.Services.AddIdentityApiEndpoints<IdentityUser>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 0;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredUniqueChars = 0;
}).AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


//roles
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roles = { "Admin", "User" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

app.MapIdentityApi<IdentityUser>();


app.AdminStatus();
app.CaloriesUtilities();

app.MapPost("/register", async (
    UserManager<IdentityUser> userManager,
    string email,
    string password) =>
{
    var user = new IdentityUser
    {
        UserName = email,
        Email = email
    };

    var result = await userManager.CreateAsync(user, password);

    if (!result.Succeeded)
        return Results.BadRequest(result.Errors);

    // Assign default role
    await userManager.AddToRoleAsync(user, "User");

    return Results.Ok("User created");
});

app.MapSwagger();
app.UseSwaggerUI();
app.MapGet("/", () => "Homepage");

app.Run();


