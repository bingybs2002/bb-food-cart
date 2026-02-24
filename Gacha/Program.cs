using Gacha.Data;
using Microsoft.EntityFrameworkCore;
using static Gacha.EndPoints.GachaLookup;
using Gacha.EndPoints;

var builder = WebApplication.CreateBuilder(args);

//hooking up to NpgSql
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration
        .GetConnectionString("Connection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.GachaItemLookup();
app.GachaCreateItem();
app.GachaDeleteItem();
app.GachaDrawItem();
app.Run();

