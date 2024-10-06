using HealthChecks.UI.Client;
using FinanceApp.Application.Exceptions;
using FinanceApp.Application.Exceptions.Handler;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace FinanceApp.WebApi;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCarter();

        services.AddExceptionHandler<CustomExceptionHandler>();
        services.AddHealthChecks()
            .AddNpgSql(configuration.GetConnectionString("DatabaseSupabase")!);

        return services;
    }

    public static WebApplication UseApiServices(this WebApplication app)
    {
        app.MapCarter();

        app.UseExceptionHandler(options => { });
        app.UseHealthChecks("/health",
            new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

        return app;
    }
}