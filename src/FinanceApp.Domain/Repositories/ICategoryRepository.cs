using FinanceApp.Domain.Models;

namespace FinanceApp.Domain.Repositories;

public interface ICategoryRepository
{
    Task<List<Category>> GetDefaultCategoriesAsync();
    Task AddCategoriesToUser(IEnumerable<Category> categories);
    Task<Category> Get(string userId, Guid? id, CancellationToken cancellationToken);
    Task<List<Category>> GetAll(string userId, CancellationToken cancellationToken);
    Task Create(Category category, CancellationToken cancellationToken);
    Task Update(Category category, CancellationToken cancellationToken);
    Task Delete(Category category, CancellationToken cancellationToken);
}