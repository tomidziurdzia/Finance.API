using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.Models.Token;
using FinanceApp.Infrastructure.Data;
using FinanceApp.Infrastructure.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IAuthService, AuthService>();

        services.AddDbContext<ApplicationDbContext>(options => 
            options.UseNpgsql(configuration.GetConnectionString("DatabaseLocalhost"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
            )
        );
        
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        return services;
    }
}