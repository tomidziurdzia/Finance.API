using FinanceApp.Domain.Abstractions;

namespace FinanceApp.Domain.Models;

public class Expense : Entity
{
    public Guid WalletId { get; set; }
    public Wallet Wallet { get; set; }
    public Guid CategoryId { get; set; }
    public Category Category { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
}