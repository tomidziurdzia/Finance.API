using FinanceApp.Domain.Models;

namespace FinanceApp.Domain.Repositories;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetDefaultCategoriesAsync();
    Task AddCategoriesToUser(IEnumerable<Category> categories); 
}