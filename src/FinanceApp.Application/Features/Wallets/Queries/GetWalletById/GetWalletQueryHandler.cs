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
        
        var totalIncome = wallet.Income.Sum(income => income.Amount);
        var totalExpense = wallet.Expense.Sum(expense => expense.Amount);
        var totalInvestment = wallet.Investment.Sum(investment => investment.Amount);
        var totalBalance = totalIncome - totalExpense - totalInvestment;

        return new WalletDto
        {
            Id = wallet.Id,
            Name = wallet.Name,
            Currency = wallet.Currency.ToString(),
            TotalBalance = totalBalance,
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
    }
}