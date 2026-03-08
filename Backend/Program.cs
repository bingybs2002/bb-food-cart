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
    options.Password.RequiredLength = 4;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
}).AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Auth Demo",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a token",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
/*
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {

                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
*/
});

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


app.MapSwagger();
app.UseSwaggerUI();
app.MapGet("/", () => "Homepage");

app.Run();


