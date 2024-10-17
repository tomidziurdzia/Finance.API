using FinanceApp.Application.Exceptions;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using FinanceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Infrastructure.Repositories;

public class TransactionRepository(ApplicationDbContext context) : ITransactionRepository
{
    public async Task<Transaction> Get(string userId, Guid id, CancellationToken cancellationToken)
    {
        try
        {
            return await context.Transactions
                .Include(t => t.Wallet)
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken)!;
        }
        catch (Exception ex)
        {
            throw new BadRequestException(ex.Message);
        }
    }

    public async Task<List<Transaction>> GetAll(string userId, CancellationToken cancellationToken)
    {
        try
        {
            return await context.Transactions
                .Include(t => t.Wallet) 
                .Include(t => t.Category)
                .Where(t => t.UserId == userId)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new BadRequestException(ex.Message);
        }
    }

    public async Task Create(Transaction transaction, CancellationToken cancellationToken)
    {
        try
        {
            await context.Transactions!.AddAsync(transaction, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error creating transaction: {ex.Message}");
        }
    }

    public async Task Update(Transaction transaction, CancellationToken cancellationToken)
    {
        try
        {
            context.Transactions!.Update(transaction);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error updating transaction: {ex.Message}");
        }
    }

    public async Task Delete(Transaction transaction, CancellationToken cancellationToken)
    {
        try
        {
            context.Transactions!.Remove(transaction);

            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting transaction: {ex.Message}");
        }
    }
}