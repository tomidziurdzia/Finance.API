using FinanceApp.Domain;
using FinanceApp.Domain.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<User>(options)
{
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) 
    {
        
        var userName = "system";
        foreach (var entry in ChangeTracker.Entries<IEntity>()) 
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.Now;
                    entry.Entity.CreatedBy = userName;
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModified = DateTime.Now;
                    entry.Entity.LastModifiedBy = userName;
                    break;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
    
    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);
        
        builder.Entity<User>().Property(x => x.NormalizedUserName).HasMaxLength(90);
        builder.Entity<IdentityRole>().Property(x => x.NormalizedName).HasMaxLength(90);
        //builder.HasDefaultSchema("financeapp-db");
    }
}