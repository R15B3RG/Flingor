using DataAccessFlingor;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connection = builder.Configuration.GetConnectionString("DogDaycareDb");

builder.Services.AddDbContext<DayCareContext>(
    options =>
    {
        options.UseSqlServer(connection);
    }
    );

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapGet("api/dog", (DayCareContext context) =>
{
    return context.Dog;
});

app.MapPost("api/dog", (DayCareContext context, Dog dog) =>
{
    context.Dog.Add(dog);
    context.SaveChanges();
});

app.MapDelete("api/dog", (DayCareContext context, Dog dog) =>
{
    context.Dog.Remove(dog);
    context.SaveChanges();
});



app.Run();

