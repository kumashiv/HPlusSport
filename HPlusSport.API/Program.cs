using HPlusSport.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Add logger
//builder.Services.AddControllers();
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(Options =>
    {
        Options.SuppressModelStateInvalidFilter = true;
    });


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ShopContext>(Options =>
{
    Options.UseInMemoryDatabase("Shop");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


//Minimal API
// connecting to existing model -InMemory database - ModelBuilderExtension
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ShopContext>();
    await db.Database.EnsureCreatedAsync();
}


app.MapGet("/products", async (ShopContext _context) =>
{
    return await _context.Products.ToArrayAsync();
});

app.MapGet("/products/{id}", async (int id, ShopContext _context) =>
{
    var product = await _context.Products.FindAsync(id);
    if (product == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(product);
});


app.MapGet("/products/available", async (ShopContext _context) =>

    Results.Ok(await _context.Products.Where(p => p.IsAvailable).ToListAsync())
    //Results.Ok(await _context.Products.Where(p => p.IsAvailable).ToArrayAsync())
);

app.Run();
