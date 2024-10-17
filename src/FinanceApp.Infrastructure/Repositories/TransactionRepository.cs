using FinanceApp.Application.Exceptions;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using FinanceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Infrastructure.Repositories;

public class TransactionRepository(ApplicationDbContext context) : ITransactionsRepository
{
    public async Task<Transaction> Get(string userId, Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var transaction = await context.Transactions!.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId, cancellationToken);
            if(transaction == null) throw new NotFoundException(nameof(Transaction), id);

            return transaction;
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
            var transactions = context.Transactions!.Where(u => u.User!.Id == userId).Include(u => u.User).AsQueryable();

            return await transactions.ToListAsync(cancellationToken);
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