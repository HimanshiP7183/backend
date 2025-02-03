using Collabzone.Models;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

// Registering CollabZoneContext with SQL Server connection string
builder.Services.AddDbContext<CollabZoneContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CollabZoneConnection")));

// Registering other services
builder.Services.AddControllers();

// Registering Swagger
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enabling middleware for Swagger and Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CollabZone API v1");
    c.RoutePrefix = string.Empty; // This makes the Swagger UI accessible at the root of the app
});

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
