using FinanceApp.Application.Exceptions;
using FinanceApp.Domain.Models;
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
            new Category { Name = "Salary", Description = "Monthly salary or income from employment." },
            new Category { Name = "Groceries", Description = "Expenses for daily food and household items." },
            new Category { Name = "Rent", Description = "Monthly payment for housing or rent." },
            new Category { Name = "Utilities", Description = "Bills for electricity, water, gas, and internet." },
            new Category { Name = "Transportation", Description = "Daily transportation costs like fuel, public transport, or car maintenance." },
            new Category { Name = "Dining Out", Description = "Expenses on eating out at restaurants or cafes." },
            new Category { Name = "Entertainment", Description = "Leisure activities such as movies, events, and hobbies." },
            new Category { Name = "Healthcare", Description = "Medical expenses like doctor visits, medication, and insurance." },
            new Category { Name = "Savings", Description = "Money set aside for future use or emergencies." },
            new Category { Name = "Subscriptions", Description = "Recurring payments for services like streaming or gym memberships." }
        };
        return await Task.FromResult(categories);
    }

    public Task AddCategoriesToUser(IEnumerable<Category> categories)
    {
        throw new NotImplementedException();
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

    public Task<Category> TryGet(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
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

    public Task Create(Category category, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Update(Category category, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Delete(Category category, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task AddCategoriesToUser(List<Category> categories)
    {
        context.Categories!.AddRange(categories);
        await context.SaveChangesAsync();
    }
}
