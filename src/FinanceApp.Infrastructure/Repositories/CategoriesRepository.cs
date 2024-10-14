using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using FinanceApp.Infrastructure.Data;
using Newtonsoft.Json;

namespace FinanceApp.Infrastructure.Repositories;

public class CategoriesRepository(ApplicationDbContext context) : ICategoriesRepository
{
    public async Task<IEnumerable<Category>> GetDefaultCategoriesAsync()
    {
        var categoryData = await File.ReadAllTextAsync("../FinanceApp.Infrastructure/Data/Extensions/Json/category.json");
        var categories = JsonConvert.DeserializeObject<List<Category>>(categoryData);

        if (categories == null)
        {
            throw new Exception("Failed to parse categories from JSON file.");
        }

        return categories;
    }

    public async Task AddCategoriesToUser(IEnumerable<Category> categories)
    {
        context.Categories!.AddRange(categories);
        await context.SaveChangesAsync();
    }
}