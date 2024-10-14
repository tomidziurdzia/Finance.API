using FinanceApp.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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
                
                if (!context.Categories!.Any())
                {
                    var categoryData = File.ReadAllText("../FinanceApp.Infrastructure/Data/Extensions/Json/category.json");
                    var categories = JsonConvert.DeserializeObject<List<Category>>(categoryData);
                    
                    foreach (var category in categories!)
                    {
                        // Crea categorías nuevas para cada usuario
                        var userCategory = new Category
                        {
                            Name = category.Name,
                            Description = category.Description,
                            UserId = user.Id // Asigna la categoría al primer usuario
                        };
                        var user2Category = new Category
                        {
                            Name = category.Name,
                            Description = category.Description,
                            UserId = user2.Id // Asigna la categoría al segundo usuario
                        };
                        await context.Categories!.AddRangeAsync(userCategory, user2Category);
                    }
                    await context.SaveChangesAsync();
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