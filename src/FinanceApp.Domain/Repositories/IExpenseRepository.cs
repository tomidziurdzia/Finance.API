using FinanceApp.Domain.Models;

namespace FinanceApp.Domain.Repositories;

public interface IExpenseRepository
{
    Task<Expense> Get(string userId, Guid id, CancellationToken cancellationToken);
    Task<List<Expense>> GetAll(string userId, CancellationToken cancellationToken);
    Task Create(Expense expense, CancellationToken cancellationToken);
    Task Update(Expense expense, CancellationToken cancellationToken);
    Task Delete(Expense expense, CancellationToken cancellationToken);
}