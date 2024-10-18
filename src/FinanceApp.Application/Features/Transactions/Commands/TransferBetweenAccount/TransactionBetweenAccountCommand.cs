using FinanceApp.Application.CQRS;

namespace FinanceApp.Application.Features.Transactions.Commands.TransferBetweenAccount;

public class TransactionBetweenAccountCommand : ICommand
{
    public Guid SourceWalletId { get; set; }
    public Guid TargetWalletId { get; set; }
    public decimal Amount { get; set; }
}