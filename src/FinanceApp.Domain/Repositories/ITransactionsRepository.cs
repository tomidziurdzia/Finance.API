using FinanceApp.Domain.Models;

namespace FinanceApp.Domain.Repositories;

public interface ITransactionsRepository
{
    Task<Transaction> Get(string userId, Guid id, CancellationToken cancellationToken);
    Task<List<Transaction>> GetAll(string userId, CancellationToken cancellationToken);
    Task Create(Transaction transaction, CancellationToken cancellationToken);
    Task Update(Transaction transaction, CancellationToken cancellationToken);
    Task Delete(Transaction transaction, CancellationToken cancellationToken);
}