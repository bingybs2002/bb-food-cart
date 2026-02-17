using Gacha.Models;
using Gacha.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//hooking up to NpgSql
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration
        .GetConnectionString("Connection")));

var app = builder.Build();

app.MapGet("/", () => "Homepage");

app.MapOpenApi();
//GET Getting all items
app.MapGet("/api/gacha", async (AppDbContext db) => 
    await db.Items.ToListAsync());

/// POST create item
app.MapPost("/api/gacha", async (AppDbContext db, ItemType items) =>
{
    db.Items.Add(items);
    await db.SaveChangesAsync();

    return Results.Created($"/api/gacha/{items.Id}", items);
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();

