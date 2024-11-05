using FinanceApp.Application.CQRS;

namespace FinanceApp.Application.Features.Transfers.Commands;

public class TransferCommand() : ICommand
{
    public Guid SourceWalletId { get; set; }
    public Guid DestinationWalletId { get; set; }
    public decimal Amount { get; set; }
}