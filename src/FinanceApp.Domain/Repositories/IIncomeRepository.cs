using FinanceApp.Domain.Models;

namespace FinanceApp.Domain.Repositories;

public interface IIncomeRepository
{
    Task<Income> Get(string userId, Guid id, CancellationToken cancellationToken);
    Task<List<Income>> GetAll(string userId, CancellationToken cancellationToken);
    Task Create(Income income, CancellationToken cancellationToken);
    Task Update(Income income, CancellationToken cancellationToken);
    Task Delete(Income income, CancellationToken cancellationToken);
}