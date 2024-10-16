using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Wallet;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Wallets.Queries.GetWallets;

public class GetWalletsQueryHandler(
    IWalletRepository walletRepository,
    UserManager<User> userManager,
    IAuthService authService)
    : IQueryHandler<GetWalletsQuery, List<WalletDto>>
{
    public async Task<List<WalletDto>> Handle(GetWalletsQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(authService.GetSessionUser());
        if (user == null) throw new UnauthorizedAccessException("User not authenticated");

        var wallets = await walletRepository.GetAll(user.Id, cancellationToken);

        return wallets.Select(wallet => new WalletDto
        {
            Id = wallet.Id,
            Name = wallet.Name,
            Currency = wallet.Currency.ToString(),
        }).ToList();
    }
}