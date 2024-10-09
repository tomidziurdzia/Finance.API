using FinanceApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Application;

public interface IApplicationDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}