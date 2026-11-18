using DroneInventory.Api.Errors;
using DroneInventory.Api.Persistence;
using DroneInventory.Api.Time;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddApiProblemDetails();
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddSingleton<ViennaCalendar>();
builder.Services.AddDbContext<DroneInventoryDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DroneInventory")));

var app = builder.Build();

app.UseApiProblemDetails();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
