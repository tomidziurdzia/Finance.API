using FinanceApp.Domain;
using FinanceApp.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FinanceApp.Infrastructure.Extensions;

public class InitialData
{
    public static async Task LoadDataAsync(
        ApplicationDbContext context,
        UserManager<User> userManager,
        ILoggerFactory loggerFactory
    ){
        try
        {
            // Cargar los usuarios de forma asíncrona
            var users = await userManager.Users.ToListAsync();

            if (!users.Any())
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
            logger.LogError(e.Message);
        }
    }
}