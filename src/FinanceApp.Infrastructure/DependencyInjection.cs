using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.Contracts.Infrastructure;
using FinanceApp.Application.Models.Token;
using FinanceApp.Domain.Repositories;
using FinanceApp.Infrastructure.Data;
using FinanceApp.Infrastructure.Repositories;
using FinanceApp.Infrastructure.Services.Auth;
using FinanceApp.Infrastructure.Services.Message;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<ICategoryRepository, CategoryRepository>();
        services.AddTransient<IWalletRepository, WalletRepository>();

        services.AddDbContext<ApplicationDbContext>(options => 
            options.UseNpgsql(configuration.GetConnectionString("DatabaseSupabase"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
            )
        );
        
        services.AddTransient<IEmailService, EmailService>();
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        return services;
    }
}