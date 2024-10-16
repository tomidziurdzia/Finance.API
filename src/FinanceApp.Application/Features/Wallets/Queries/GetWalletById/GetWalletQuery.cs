using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Wallet;

namespace FinanceApp.Application.Features.Wallets.Queries.GetWalletById;

public class GetWalletQuery : IQuery<WalletDto>
{
    public Guid WalletId { get; set; }

    public GetWalletQuery(Guid walletId)
    {
        WalletId = walletId;
    }
}