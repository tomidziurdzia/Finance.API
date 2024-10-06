using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Application.Data;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}