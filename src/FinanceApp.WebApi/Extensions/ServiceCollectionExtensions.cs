using Carter;
using FinanceApp.Application.Behaviours;
using FinanceApp.Application.Contracts;
using FinanceApp.Application.Exceptions.Handler;
using FinanceApp.Application.Services;
using FinanceApp.Domain.Repositories;
using FinanceApp.Infrastructure;
using FinanceApp.Infrastructure.Repositories;
using FinanceApp.Infrastructure.Services;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;

namespace FinanceApp.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFinanceAppServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddApplicationServices() // Consolidamos aquí la configuración de la aplicación
            .AddWebApi(configuration)
            .AddInfrastructureServices(configuration)
            .AddApiServices(configuration);

        return services;
    }
    
    private static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Registrar MediatR y comportamientos (Validation, Logging, etc.)
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(FinanceApp.Application.Features.Users.Queries.GetUsers.GetUsersQueryHandler).Assembly);
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        // Registrar el repositorio y el servicio de usuarios
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
    
    private static IServiceCollection AddWebApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "FinanceApp API",
                Version = "v1",
                Description = "API for managing users, transactions, and other finance operations."
            });
        });
    
        return services;
    }

    private static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Carter para manejo de rutas
        services.AddCarter();

        // Manejador de excepciones
        services.AddExceptionHandler<CustomExceptionHandler>();

        // Health Checks con PostgreSQL
        services.AddHealthChecks()
            .AddNpgSql(configuration.GetConnectionString("DatabaseSupabase")!);

        return services;
    }

    public static WebApplication UseApiServices(this WebApplication app)
    {
        // Mapear rutas de Carter
        app.MapCarter();

        // Manejo de excepciones
        app.UseExceptionHandler(options => { });

        // Configurar Health Checks
        app.UseHealthChecks("/health",
            new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

        return app;
    }
}
