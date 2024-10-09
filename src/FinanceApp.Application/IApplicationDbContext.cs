using FinanceApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Application;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}