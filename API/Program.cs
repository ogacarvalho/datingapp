// Este arquivo é o "EntryPoint" o middleware que fará a requisição trabalhar como queremos.
using API.Data;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(opt => 
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapControllers(); // Middleware que dirá aonde esta o endpoint procurado.

app.Run();
