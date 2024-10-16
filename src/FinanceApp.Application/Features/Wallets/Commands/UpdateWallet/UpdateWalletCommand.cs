using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Wallet;

namespace FinanceApp.Application.Features.Wallets.Commands.UpdateWallet;

public class UpdateWalletCommand : ICommand<WalletDto>
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public UpdateWalletCommand(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}