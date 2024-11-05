using FinanceApp.Domain.Models;

namespace FinanceApp.Domain.Repositories;

public interface IInvestmentRepository
{
    Task<Investment> Get(string userId, Guid id, CancellationToken cancellationToken);
    Task<List<Investment>> GetAll(string userId, CancellationToken cancellationToken);
    Task Create(Investment investment, CancellationToken cancellationToken);
    Task Update(Investment investment, CancellationToken cancellationToken);
    Task Delete(Investment investment, CancellationToken cancellationToken);
}