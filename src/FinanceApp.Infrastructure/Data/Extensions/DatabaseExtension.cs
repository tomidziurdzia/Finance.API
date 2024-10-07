using FinanceApp.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FinanceApp.Infrastructure.Data.Extensions;

public static class DatabaseExtension
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Database.MigrateAsync().GetAwaiter().GetResult();

        await SeedAsync(scope.ServiceProvider);
    }
    
    private static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        // Obtén el loggerFactory, context y userManager
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

        await SeedUserAsync(context, userManager, loggerFactory);
    }
    
    private static async Task SeedUserAsync(ApplicationDbContext context, UserManager<User> userManager, ILoggerFactory loggerFactory)
    {
        await InitialData.LoadDataAsync(context, userManager, loggerFactory);
    }
}