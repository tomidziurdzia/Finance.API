using FinanceApp.Domain.Models;

namespace FinanceApp.Domain.Repositories;

public interface ICategoriesRepository
{
    Task<IEnumerable<Category>> GetDefaultCategoriesAsync();
    Task AddCategoriesToUser(IEnumerable<Category> categories); 
}