using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddAplicationServices(this IServiceCollection services, IConfiguration config)
        {        
            services.AddDbContext<DataContext>(opt => 
            {
                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            services.AddCors(); // Estamos habilitando o serviço de Cors (Cros Origins Resorces)
            services.AddScoped<ITokenService, TokenService>(); // Criando o serviço de autenticação por TOKEN. (JWT)

            return services;
        }
    }
}