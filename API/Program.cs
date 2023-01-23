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

builder.Services.AddCors(); // Estamos habilitando o serviço de Cors (Cros Origins Resorces)

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200")); // Habilitando que o localhost específico, (front) pode consumir a api.

app.MapControllers(); // Middleware que dirá aonde esta o endpoint procurado.

app.Run();
