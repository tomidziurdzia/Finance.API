using FinanceApp.Domain.Models;

namespace FinanceApp.Domain.Repositories;

public interface IWalletRepository
{
    Task<Wallet> Get(string userId, Guid id, CancellationToken cancellationToken);
    Task<List<Wallet>> GetAll(string userId, CancellationToken cancellationToken);
    Task Create(Wallet wallet, CancellationToken cancellationToken);
    Task Update(Wallet wallet, CancellationToken cancellationToken);
    Task Delete(Wallet wallet, CancellationToken cancellationToken);
}