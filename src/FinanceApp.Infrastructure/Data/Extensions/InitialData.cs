using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

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
            // Verifica si ya hay usuarios
            if (!userManager.Users.Any())
            {
                var user = new User
                {
                    Name = "Tomas",
                    Lastname = "Dziurdzia",
                    Email = "tomidziurdzia@gmail.com",
                    UserName = "tomidziurdzia",
                };
                await userManager.CreateAsync(user, "Walter@960");

                var user2 = new User
                {
                    Name = "Xime",
                    Lastname = "Apel",
                    Email = "ximeapel@gmail.com",
                    UserName = "ximeapel",
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