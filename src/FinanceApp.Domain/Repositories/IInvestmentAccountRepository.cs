using FinanceApp.Domain.Models;

namespace FinanceApp.Domain.Repositories;

public interface IInvestmentAccountRepository
{
    Task<InvestmentAccount> Get(string userId, Guid id, CancellationToken cancellationToken);
    Task<List<InvestmentAccount>> GetAll(string userId, CancellationToken cancellationToken);
    Task Create(InvestmentAccount wallet, CancellationToken cancellationToken);
    Task Update(InvestmentAccount wallet, CancellationToken cancellationToken);
    Task Delete(InvestmentAccount wallet, CancellationToken cancellationToken);
}