using FinanceApp.Application.CQRS;

namespace FinanceApp.Application.Features.Transfers.Commands.TransferFromInvestmentToWallet;

public class TransferFromInvestmentCommand : ICommand
{
    public Guid InvestmentAccountId { get; set; }
    public Guid DestinationWalletId { get; set; }
    public decimal Amount { get; set; }
}