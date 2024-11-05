using FinanceApp.Application.Exceptions;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using FinanceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Infrastructure.Repositories;

public class InvestmentRepository(ApplicationDbContext context) : IInvestmentRepository
{
    public async Task<Investment> Get(string userId, Guid id, CancellationToken cancellationToken)
    {
        try
        {
            return await context.Investments
                .Include(t => t.Wallet)
                .Include(t => t.Category)
                .FirstOrDefaultAsync(i => i.UserId == userId && i.Id == id, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new BadRequestException(ex.Message);
        }
    }

    public async Task<List<Investment>> GetAll(string userId, CancellationToken cancellationToken)
    {
        try
        {
            return await context.Investments
                .Include(t => t.Wallet)
                .Include(t => t.Category)
                .Where(inv => inv.UserId == userId)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new BadRequestException(ex.Message);
        }
    }

    public async Task Create(Investment investment, CancellationToken cancellationToken)
    {
        try
        {
            await context.Investments.AddAsync(investment, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error creating investment: {ex.Message}");
        }
    }

    public async Task Update(Investment investment, CancellationToken cancellationToken)
    {
        try
        {
            context.Investments.Update(investment);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error updating investment: {ex.Message}");
        }
    }

    public async Task Delete(Investment investment, CancellationToken cancellationToken)
    {
        try
        {
            context.Investments.Remove(investment);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting investment: {ex.Message}");
        }
    }
}
