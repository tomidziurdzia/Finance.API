using FinanceApp.Application.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace FinanceApp.Infrastructure.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : IdentityDbContext<User>(options), IApplicationDbContext
    {
        public new DbSet<User> Users => Set<User>();

        public new async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}