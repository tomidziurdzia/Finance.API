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
    : IQueryHandler<GetWalletsQuery, WalletsDto>
{
    public async Task<WalletsDto> Handle(GetWalletsQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(authService.GetSessionUser());
        if (user == null) throw new UnauthorizedAccessException("User not authenticated");

        var wallets = await walletRepository.GetAll(user.Id, cancellationToken);

        var walletDtos = wallets.Select(wallet => 
        {
            var total = wallet.Transactions.Sum(t => t.Amount);

            return new WalletDto
            {
                Id = wallet.Id,
                Name = wallet.Name,
                Currency = wallet.Currency.ToString(),
                Total = total
            };
        }).ToList();

        var totalGlobal = walletDtos.Sum(w => w.Total);

        return new WalletsDto
        {
            Wallets = walletDtos,
            Total = totalGlobal
        };
    }
}