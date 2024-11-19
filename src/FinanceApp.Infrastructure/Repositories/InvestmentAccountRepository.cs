using FinanceApp.Application.Exceptions;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using FinanceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Infrastructure.Repositories;

public class InvestmentAccountRepository(ApplicationDbContext context) : IInvestmentAccountRepository
{
    public async Task<InvestmentAccount> Get(string userId, Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var investmentAccount = await context.InvestmentAccounts
                .Include(w => w.Income).ThenInclude(i => i.Category)
                .Include(w => w.Expense).ThenInclude(i => i.Category)
                .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId, cancellationToken);
            
            if (investmentAccount == null) throw new NotFoundException(nameof(InvestmentAccount), id);

            return investmentAccount;
        }
        catch (Exception ex)
        {
            throw new BadRequestException(ex.Message);
        }
    }

    public async Task<List<InvestmentAccount>> GetAll(string userId, CancellationToken cancellationToken)
    {
        try
        {
            return await context.InvestmentAccounts
                .Where(w => w.UserId == userId)
                .Include(w => w.Income).ThenInclude(i => i.Category)
                .Include(w => w.Expense).ThenInclude(i => i.Category)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new BadRequestException(ex.Message);
        }
    }

    public async Task Create(InvestmentAccount investmentAccount, CancellationToken cancellationToken)
    {
        try
        {
            await context.InvestmentAccounts.AddAsync(investmentAccount, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error creating wallet: {ex.Message}");
        }
    }

    public async Task Update(InvestmentAccount investmentAccount, CancellationToken cancellationToken)
    {
        try
        {
            context.InvestmentAccounts.Update(investmentAccount);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error updating wallet: {ex.Message}");
        }
    }

    public async Task Delete(InvestmentAccount investmentAccount, CancellationToken cancellationToken)
    {
        try
        {
            context.InvestmentAccounts.Remove(investmentAccount);

            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting wallet: {ex.Message}");
        }
    }
}