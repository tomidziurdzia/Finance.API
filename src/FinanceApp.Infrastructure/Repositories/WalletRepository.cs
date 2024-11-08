using FinanceApp.Application.Exceptions;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using FinanceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Infrastructure.Repositories;

public class WalletRepository(ApplicationDbContext context) : IWalletRepository
{
    public async Task<Wallet> Get(string userId, Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var wallet = await context.Wallets
                .Include(w => w.Income).ThenInclude(i => i.Category)
                .Include(w => w.Expense).ThenInclude(i => i.Category)
                .Include(w => w.Investment).ThenInclude(i => i.Category)
                .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId, cancellationToken);

            if (wallet == null) throw new NotFoundException(nameof(Wallet), id);

            return wallet;
        }
        catch (Exception ex)
        {
            throw new BadRequestException(ex.Message);
        }
    }

    public async Task<List<Wallet>> GetAll(string userId, CancellationToken cancellationToken)
    {
        try
        {
            return await context.Wallets
                .Where(w => w.UserId == userId)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new BadRequestException(ex.Message);
        }
    }

    public async Task Create(Wallet wallet, CancellationToken cancellationToken)
    {
        try
        {
            await context.Wallets.AddAsync(wallet, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error creating wallet: {ex.Message}");
        }
    }

    public async Task Update(Wallet wallet, CancellationToken cancellationToken)
    {
        try
        {
            context.Wallets.Update(wallet);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error updating wallet: {ex.Message}");
        }
    }

    public async Task Delete(Wallet wallet, CancellationToken cancellationToken)
    {
        try
        {
            context.Wallets.Remove(wallet);

            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting wallet: {ex.Message}");
        }
    }
}