using FinanceApp.Application.Models.Authorization;
using FinanceApp.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace FinanceApp.Infrastructure.Data.Extensions;

public class InitialData
{
    public static async Task LoadDataAsync(
        ApplicationDbContext context,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager,
        ILoggerFactory loggerFactory
    )
    {
        try
        {
            if(!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole(Role.ADMIN));
                await roleManager.CreateAsync(new IdentityRole(Role.USER));
            }
            
            // Verifica si ya hay usuarios
            if (!userManager.Users.Any())
            {
                var user = new User
                {
                    Name = "Tomas",
                    Lastname = "Dziurdzia",
                    UserName = "tomidziurdzia",
                    Email = "tomidziurdzia@gmail.com"
                };
                var result = await userManager.CreateAsync(user, "Walter@960");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Role.ADMIN);
                }

                var user2 = new User
                {
                    Name = "Xime",
                    Lastname = "Apel",
                    UserName = "ximeapel",
                    Email = "ximeapel@gmail.com"
                };
                var result2 = await userManager.CreateAsync(user2, "Walter@960");

                if (result2.Succeeded)
                {
                    await userManager.AddToRoleAsync(user2, Role.USER);
                }
            }

        }
        catch (Exception e)
        {
            var logger = loggerFactory.CreateLogger<InitialData>();
            logger.LogError(e, "An error occurred while seeding the database");
        }
    }
}