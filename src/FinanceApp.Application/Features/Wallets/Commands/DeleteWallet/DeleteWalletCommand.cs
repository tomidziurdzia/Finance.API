using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Wallet;
using MediatR;

namespace FinanceApp.Application.Features.Wallets.Commands.DeleteWallet;

public class DeleteWalletCommand : ICommand<Unit>
{
    public Guid Id { get; set; }

    public DeleteWalletCommand(Guid id)
    {
        Id = id;
    }
}