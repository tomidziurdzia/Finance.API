using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Wallet;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Wallets.Commands.DeleteWallet;

public class DeleteWalletCommandHandler(
    IWalletRepository walletRepository,
    UserManager<User> userManager,
    IAuthService authService)
    : ICommandHandler<DeleteWalletCommand, Unit>
{
    public async Task<Unit> Handle(DeleteWalletCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(authService.GetSessionUser());
        if (user == null) throw new UnauthorizedAccessException("User not authenticated");

        var wallet = await walletRepository.Get(user.Id ,request.Id, cancellationToken);

        await walletRepository.Delete(wallet, cancellationToken);

        return Unit.Value;
    }
}