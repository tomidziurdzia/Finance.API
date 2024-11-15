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
            var totalIncome = wallet.Income.Sum(income => income.Amount);
            var totalExpense = wallet.Expense.Sum(expense => expense.Amount);
            var totalInvestment = wallet.Investment.Sum(investment => investment.Amount);
            var totalBalance = totalIncome - totalExpense - totalInvestment;
            
            return new SimpleWalletDto
            {
                Id = wallet.Id,
                Name = wallet.Name,
                Total = totalBalance,
                Currency = wallet.Currency.ToString(),
            };
        }).ToList();
        
        return new WalletsDto
        {
            Wallets = walletDtos,
        };
    }
}