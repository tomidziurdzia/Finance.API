using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Wallet;
using FinanceApp.Application.Exceptions;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Wallets.Commands.UpdateWallet;

public class UpdateWalletCommandHandler(
    IWalletRepository walletRepository,
    UserManager<User> userManager,
    IAuthService authService)
    : ICommandHandler<UpdateWalletCommand, WalletDto>
{
    public async Task<WalletDto> Handle(UpdateWalletCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(authService.GetSessionUser());
        if (user == null) throw new UnauthorizedAccessException("User not authenticated");

        var wallet = await walletRepository.Get(user.Id, request.Id, cancellationToken);

        if (wallet.UserId != user.Id)
        {
            throw new UnauthorizedAccessException("You do not have access to this wallet.");
        }

        wallet.Name = request.Name;

        await walletRepository.Update(wallet, cancellationToken);

        return new WalletDto
        {
            Id = wallet.Id,
            Name = wallet.Name,
            Currency = wallet.Currency.ToString(),
        };
    }
}