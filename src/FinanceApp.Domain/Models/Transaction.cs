using FinanceApp.Domain.Abstractions;
using FinanceApp.Domain.Models.Enums;

namespace FinanceApp.Domain.Models;

public class Transaction : Entity
{
    public Guid WalletId { get; set; }
    public Wallet Wallet { get; set; }
    public Guid? CategoryId { get; set; }
    public Category? Category { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public DateTime? CreatedAt { get; set; }
}