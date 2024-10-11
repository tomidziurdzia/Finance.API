using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using FinanceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;

    public CategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Category>> GetDefaultCategoriesAsync()
    {
        return await _context.Categories!.ToListAsync();
    }

    public async Task AddCategoriesToUser(IEnumerable<Category> categories)
    {
        _context.Categories!.AddRange(categories);
        await _context.SaveChangesAsync();
    }
}

public interface ICategoryRepository
{
}