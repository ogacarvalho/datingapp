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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddAplicationServices(builder.Configuration);  // Extension Methods, é um método estático que permite componentizar as responsabilidades.
builder.Services.AddIdentityServices(builder.Configuration);    // Extensão da configuração do Autenticador da API

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200")); // Habilitando que o localhost específico, (front) pode consumir a api.

app.UseAuthentication(); // Middleware:Você tem um Token Válido?                                 !Precisar ESPECIFICAMENTE estar nesta posição.
app.UseAuthorization(); // Middleware:OK, deixa eu ver o que você pode fazer....                 !Precisa ESPECIFICAMENTE estar nesta posição.

app.MapControllers(); // Middleware que dirá aonde esta o endpoint procurado.

app.Run();
