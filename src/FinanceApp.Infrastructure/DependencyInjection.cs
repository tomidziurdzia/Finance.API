using FinanceApp.Application.Interfaces.Repositories;
using FinanceApp.Application.Models.Token;
using FinanceApp.Infrastructure.Data.Interceptors;
using FinanceApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        return services;
    }
}