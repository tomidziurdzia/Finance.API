using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Wallet;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Models.Enums;
using FinanceApp.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Wallets.Queries.GetWalletsTotal;

public class GetWalletTotalsQueryHandler(
    IWalletRepository walletRepository,
    UserManager<User> userManager,
    IAuthService authService)
    : IQueryHandler<GetWalletTotalsQuery, WalletTotalsResponseDto>
{
    public async Task<WalletTotalsResponseDto> Handle(GetWalletTotalsQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(authService.GetSessionUser());
        if (user == null) throw new UnauthorizedAccessException("User not authenticated");

        var wallets = await walletRepository.GetAll(user.Id, cancellationToken);

        var walletTotals = wallets.Select(wallet =>
        {
            var totalIncome = wallet.Transactions
                .Where(t => t.Type == TransactionType.Income)
                .Sum(t => t.Amount);

            var totalExpense = wallet.Transactions
                .Where(t => t.Type == TransactionType.Expense)
                .Sum(t => t.Amount);

            return new WalletTotalDto
            {
                Id = wallet.Id,
                Total = totalIncome - totalExpense,
                Income = totalIncome,
                Expense = totalExpense
            };
        }).ToList();

        var totalSum = walletTotals.Sum(w => w.Total);
        var totalIncomeSum = walletTotals.Sum(w => w.Income);
        var totalExpenseSum = walletTotals.Sum(w => w.Expense);

        return new WalletTotalsResponseDto
        {
            Wallets = walletTotals,
            Total = totalSum,
            Income = totalIncomeSum,
            Expense = totalExpenseSum
        };
    }
}