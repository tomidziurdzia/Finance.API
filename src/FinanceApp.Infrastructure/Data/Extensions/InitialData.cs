using FinanceApp.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace FinanceApp.Infrastructure.Data.Extensions;

public class InitialData
{
    public static async Task LoadDataAsync(
        ApplicationDbContext context,
        UserManager<User> userManager,
        ILoggerFactory loggerFactory
    )
    {
        try
        {
            if (!userManager.Users.Any())
            {
                var user = new User
                {
                    Name = "Tomas",
                    Lastname = "Dziurdzia",
                    UserName = "tomidziurdzia",
                    Email = "tomidziurdzia@gmail.com"
                };
                await userManager.CreateAsync(user, "Walter@960");

                var user2 = new User
                {
                    Name = "Xime",
                    Lastname = "Apel",
                    UserName = "ximeapel",
                    Email = "ximeapel@gmail.com"
                };
                await userManager.CreateAsync(user2, "Walter@960");
            }

        }
        catch (Exception e)
        {
            var logger = loggerFactory.CreateLogger<InitialData>();
            logger.LogError(e, "An error occurred while seeding the database");
        }
    }
}