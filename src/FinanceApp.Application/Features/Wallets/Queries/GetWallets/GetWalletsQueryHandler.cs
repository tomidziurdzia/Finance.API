using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Transaction;
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
        return new WalletDto
        {
            Id = wallet.Id,
            Name = wallet.Name,
            Currency = wallet.Currency.ToString(),
            Transactions = wallet.Income
                .Select(income => new TransactionWalletDto
                {
                    Id = income.Id,
                    CategoryId = income.CategoryId,
                    CategoryName = income.Category.Name,
                    Amount = income.Amount,
                    Description = income.Description,
                    Date = income.CreatedAt,
                    Type = "Income"
                })
                .Concat(wallet.Expense.Select(expense => new TransactionWalletDto
                {
                    Id = expense.Id,
                    CategoryId = expense.CategoryId,
                    CategoryName = expense.Category.Name,
                    Amount = expense.Amount,
                    Description = expense.Description,
                    Date = expense.CreatedAt,
                    Type = "Expense"
                }))
                .Concat(wallet.Investment.Select(investment => new TransactionWalletDto
                {
                    Id = investment.Id,
                    CategoryId = investment.CategoryId,
                    CategoryName = investment.Category.Name,
                    Amount = investment.Amount,
                    Description = investment.Description,
                    Date = investment.CreatedAt,
                    Type = "Investment"
                }))
                .OrderByDescending(t => t.Date)
                .ToList()
        };
    }).ToList();

        return new WalletsDto
        {
            Wallets = walletDtos,
        };
    }
}