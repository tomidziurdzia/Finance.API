using FinanceApp.Application.Exceptions;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using FinanceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Infrastructure.Repositories;

public class ExpenseRepository(ApplicationDbContext context) : IExpenseRepository
{
    public async Task<Expense> Get(string userId, Guid id, CancellationToken cancellationToken)
    {
        try
        {
            return await context.Expenses
                .Include(t => t.Wallet)
                .Include(t => t.Category)
                .FirstOrDefaultAsync(i => i.UserId == userId && i.Id == id, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new BadRequestException(ex.Message);
        }
    }

    public async Task<List<Expense>> GetAll(string userId, CancellationToken cancellationToken)
    {
        try
        {
            return await context.Expenses
                .Include(t => t.Wallet)
                .Include(t => t.Category)
                .Where(e => e.UserId == userId)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new BadRequestException(ex.Message);
        }
    }

    public async Task Create(Expense expense, CancellationToken cancellationToken)
    {
        try
        {
            await context.Expenses.AddAsync(expense, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error creating expense: {ex.Message}");
        }
    }

    public async Task Update(Expense expense, CancellationToken cancellationToken)
    {
        try
        {
            context.Expenses.Update(expense);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error updating expense: {ex.Message}");
        }
    }

    public async Task Delete(Expense expense, CancellationToken cancellationToken)
    {
        try
        {
            context.Expenses.Remove(expense);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting expense: {ex.Message}");
        }
    }
}