// Este arquivo é o "EntryPoint" o middleware que fará a requisição trabalhar como queremos.
/* Quando a requisição bate no endpoint e o framework instancia o controlador, o controlador ou o próprio framework olha estas dependências e diz
Ahá! Preciso criar instâncias destes servicos aqui! 

Então, quando o controlador é liberado no final de requisição HTTP, então qualquer dependência (serviços) também será liberado a depender
do tipo de comportamento que escolhermos (Transient, Scoped ou Singleton)
→ Scoped dura o periodo que a requisição durar. (Geralmente é default)
→ Transient dura enquanto o usuário usar.
→ Singleton, que é criado quando a aplicação é iniciada nunca desliga até a aplicação ser desligada. (Tem que tomar cuidado com a memoria! Ideal para serviços de Cache.

*/
using API.Data;
using API.Services;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(opt => 
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddCors(); // Estamos habilitando o serviço de Cors (Cros Origins Resorces)
builder.Services.AddScoped<ITokenService, TokenService>(); // Criando o serviço de autenticação por TOKEN. (JWT)

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200")); // Habilitando que o localhost específico, (front) pode consumir a api.

app.MapControllers(); // Middleware que dirá aonde esta o endpoint procurado.

app.Run();
