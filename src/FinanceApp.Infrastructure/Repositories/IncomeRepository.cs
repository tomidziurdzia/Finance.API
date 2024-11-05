using FinanceApp.Application.Exceptions;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using FinanceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Infrastructure.Repositories;

public class IncomeRepository(ApplicationDbContext context) : IIncomeRepository
{
    public async Task<Income> Get(string userId, Guid id, CancellationToken cancellationToken)
    {
        try
        {
            return await context.Incomes
                .Include(t => t.Wallet)
                .Include(t => t.Category)
                .FirstOrDefaultAsync(i => i.UserId == userId && i.Id == id, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new BadRequestException(ex.Message);
        }
    }

    public async Task<List<Income>> GetAll(string userId, CancellationToken cancellationToken)
    {
        try
        {
            return await context.Incomes
                .Include(t => t.Wallet)
                .Include(t => t.Category)
                .Where(i => i.UserId == userId)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new BadRequestException(ex.Message);
        }
    }

    public async Task Create(Income income, CancellationToken cancellationToken)
    {
        try
        {
            await context.Incomes.AddAsync(income, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error creating income: {ex.Message}");
        }
    }

    public async Task Update(Income income, CancellationToken cancellationToken)
    {
        try
        {
            context.Incomes.Update(income);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error updating income: {ex.Message}");
        }
    }

    public async Task Delete(Income income, CancellationToken cancellationToken)
    {
        try
        {
            context.Incomes.Remove(income);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting income: {ex.Message}");
        }
    }
}
