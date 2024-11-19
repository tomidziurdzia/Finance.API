using FinanceApp.Application.CQRS;

namespace FinanceApp.Application.Features.Transfers.Commands.TransferWalletToInvestmentAccount;

public class TransferToInvestmentCommand : ICommand
{
    public Guid SourceWalletId { get; set; }
    public Guid InvestmentAccountId { get; set; }
    public decimal Amount { get; set; }
}