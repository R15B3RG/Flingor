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
    var dogs = new List<Dog>
        (
            context.Dog.Select(
                d => new Dog()
                {
                    Name = d.Name,
                    Owners = new List<Owner>
                    (
                        d.Owners.Select(o => new Owner()
                        {
                            Name = o.Name
                        }))
                }
                )
        );
    return dogs;
});

app.MapPost("api/dog", (DayCareContext context, Dog dog) =>
{
    context.Dog.Add(dog);
    context.SaveChanges();
});

app.MapDelete("api/dog/{id}", (DayCareContext context, int id) =>
{
    var dog = context.Dog.Find(id);
    if (dog != null)
    {
        context.Dog.Remove(dog);
        context.SaveChanges();
    }
    
});

app.MapPut("api/dog/{id}", (DayCareContext context, Dog newDog, int id) =>
{
    var dog = context.Dog.Find(id);

    if (dog != null)
    {
        dog = newDog;
    }

    context.SaveChanges();
});

app.MapPut("api/dog/owner", (DayCareContext context, int dogId, int ownerId) =>
{
    var owner = context.Owner.Find(ownerId);
    var dog = context.Dog.Find(dogId);

    if (owner != null)
    {
        if (dog != null)
        {
            dog.Owners.Add(owner);
            context.SaveChanges();
        }
    }
});

/////////////
app.MapGet("api/owner", (DayCareContext context) =>
{
    return context.Owner;
});

app.MapPost("api/owner", (DayCareContext context, Owner owner) =>
{
    context.Owner.Add(owner);
    context.SaveChanges();
});

app.MapDelete("api/owner/{id}", (DayCareContext context, int id) =>
{
    var owner = context.Owner.Find(id);
    if (owner != null)
    {
        context.Owner.Remove(owner);
        context.SaveChanges();
    }

});

app.MapPut("api/owner/{id}", (DayCareContext context, Owner newOwner, int id) =>
{
    var owner = context.Owner.Find(id);

    if (owner != null)
    {
        owner = newOwner;
    }

    context.SaveChanges();
});

app.MapPut("api/owner/dogs", (DayCareContext context, int dogId, int ownerId) =>
{
    var owner = context.Owner.Find(ownerId);
    var dog = context.Dog.Find(dogId);

    if (owner != null)
    {
        if (dog != null)
        {
            owner.Dogs.Add(dog);
            context.SaveChanges();
        }
    }
});

app.Run();

