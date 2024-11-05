using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Wallet;
using FinanceApp.Application.Exceptions;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Models.Enums;
using FinanceApp.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Wallets.Queries.GetWalletById;

public class GetWalletQueryHandler(
    IWalletRepository walletRepository,
    UserManager<User> userManager,
    IAuthService authService)
    : IQueryHandler<GetWalletQuery, WalletDto>
{
    public async Task<WalletDto> Handle(GetWalletQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(authService.GetSessionUser());
        if (user == null) throw new UnauthorizedAccessException("User not authenticated");

        var wallet = await walletRepository.Get(user.Id, request.WalletId, cancellationToken);

        if (wallet == null)
        {
            throw new NotFoundException(nameof(wallet), request.WalletId);
        }

        return new WalletDto
        {
            Id = wallet.Id,
            Name = wallet.Name,
            Currency = wallet.Currency.ToString(),
        };
    }
}