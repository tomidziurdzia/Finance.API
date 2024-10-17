using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Transaction;
using FinanceApp.Domain.Models.Enums;

namespace FinanceApp.Application.Features.Transactions.Commands.CreateTransaction;

public class CreateTransactionCommand : ICommand<TransactionDto>
{
    public Guid WalletId { get; set; }
    public Guid CategoryId { get; set; }
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
}