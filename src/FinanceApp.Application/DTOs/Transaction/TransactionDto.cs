using FinanceApp.Application.DTOs.Category;
using FinanceApp.Application.DTOs.Wallet;
using FinanceApp.Domain.Models.Enums;

namespace FinanceApp.Application.DTOs.Transaction;

public class TransactionDto
{
    public Guid? Id { get; set; }
    public WalletDto Wallet { get; set; }
    public CategoryDto Category { get; set; }
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
}