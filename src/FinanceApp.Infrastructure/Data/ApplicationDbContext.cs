using FinanceApp.Application;
using FinanceApp.Domain.Abstractions;
using FinanceApp.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        }
        
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) 
        {
            var userName = "system";

            foreach (var entry in ChangeTracker.Entries<Entity>()) 
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
        
        public DbSet<Category> Categories { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Investment> Investments { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Category>()
                .HasOne(c => c.User)
                .WithMany(u => u.Categories)
                .HasForeignKey(c => c.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<Category>()
                .Property(c => c.ParentType)
                .HasConversion<string>();

            builder.Entity<Category>()
                .Property(c => c.Type)
                .HasConversion<string>();

            builder.Entity<Wallet>()
                .HasOne(w => w.User)
                .WithMany(u => u.Wallets)
                .HasForeignKey(u => u.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Configuration Income
            builder.Entity<Income>()
                .HasOne(i => i.User)
                .WithMany(u => u.Income)
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Income>()
                .HasOne(i => i.Wallet)
                .WithMany(w => w.Income)
                .HasForeignKey(i => i.WalletId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Income>()
                .HasOne(i => i.Category)
                .WithMany(c => c.Incomes)
                .HasForeignKey(i => i.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Income>()
                .Property(i => i.CreatedAt)
                .HasDefaultValue(null);

            // Configuration Expense
            builder.Entity<Expense>()
                .HasOne(e => e.User)
                .WithMany(u => u.Expense)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Expense>()
                .HasOne(e => e.Wallet)
                .WithMany(w => w.Expense)
                .HasForeignKey(e => e.WalletId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Expense>()
                .HasOne(e => e.Category)
                .WithMany(c => c.Expenses)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Expense>()
                .Property(e => e.CreatedAt)
                .HasDefaultValue(null);

            // Configuration Investment
            builder.Entity<Investment>()
                .HasOne(inv => inv.User)
                .WithMany(u => u.Investment)
                .HasForeignKey(inv => inv.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Investment>()
                .HasOne(inv => inv.Wallet)
                .WithMany(w => w.Investment)
                .HasForeignKey(inv => inv.WalletId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Investment>()
                .HasOne(inv => inv.Category)
                .WithMany(c => c.Investments)
                .HasForeignKey(inv => inv.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Investment>()
                .Property(inv => inv.CreatedAt)
                .HasDefaultValue(null);
        }

    }
}