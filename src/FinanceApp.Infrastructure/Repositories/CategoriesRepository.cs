using FinanceApp.Application.Exceptions;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Models.Enums;
using FinanceApp.Domain.Repositories;
using FinanceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Infrastructure.Repositories;

public class CategoriesRepository(ApplicationDbContext context) : ICategoriesRepository
{
    public async Task<List<Category>> GetDefaultCategoriesAsync()
    {
        var categories = new List<Category>
        {
            new Category { Name = "Salary", Description = "Monthly salary or income from employment.", Type = CategoryType.Income},
            new Category { Name = "Groceries", Description = "Expenses for daily food and household items." , Type = CategoryType.Expense},
            new Category { Name = "Rent", Description = "Monthly payment for housing or rent.", Type = CategoryType.Expense},
            new Category { Name = "Utilities", Description = "Bills for electricity, water, gas, and internet." , Type = CategoryType.Expense},
            new Category { Name = "Transportation", Description = "Daily transportation costs like fuel, public transport, or car maintenance." , Type = CategoryType.Expense},
            new Category { Name = "Dining Out", Description = "Expenses on eating out at restaurants or cafes." , Type = CategoryType.Expense},
            new Category { Name = "Entertainment", Description = "Leisure activities such as movies, events, and hobbies." , Type = CategoryType.Expense},
            new Category { Name = "Healthcare", Description = "Medical expenses like doctor visits, medication, and insurance." , Type = CategoryType.Expense},
            new Category { Name = "Savings", Description = "Money set aside for future use or emergencies." , Type = CategoryType.Investment},
            new Category { Name = "Subscriptions", Description = "Recurring payments for services like streaming or gym memberships." , Type = CategoryType.Expense}
        };
        return await Task.FromResult(categories);
    }

    public async Task AddCategoriesToUser(IEnumerable<Category> categories)
    {
        try
        {
            context.Categories!.AddRange(categories);
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error adding categories to user: {ex.Message}");
        }
    }

    public async Task<Category> Get(string userId, Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var category = await context.Categories!.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId, cancellationToken);
            if(category == null) throw new NotFoundException(nameof(Category), id);

            return category;
        }
        catch (Exception ex)
        {
            throw new BadRequestException(ex.Message);
        }
    }
    public async Task<List<Category>> GetAll(string userId, CancellationToken cancellationToken)
    {
        try
        {
            var categories = context.Categories!.Where(u => u.User!.Id == userId).Include(u => u.User).AsQueryable();

            return await categories.ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new BadRequestException(ex.Message);
        }
    }

    public async Task Create(Category category, CancellationToken cancellationToken)
    {
        try
        {
            await context.Categories!.AddAsync(category, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error creating category: {ex.Message}");
        }
    }

    public async Task Update(Category category, CancellationToken cancellationToken)
    {
        try
        {
            context.Categories!.Update(category);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error updating category: {ex.Message}");
        }
    }

    public async Task Delete(Category category, CancellationToken cancellationToken)
    {
        try
        {
            context.Categories!.Remove(category);

            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting category: {ex.Message}");
        }
    }
}
