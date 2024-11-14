using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Income;
using FinanceApp.Application.DTOs.Investment;
using FinanceApp.Application.DTOs.Transaction;
using FinanceApp.Application.Features.Investments.Queries.GetAll;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Overview.Queries.GetTotal;

public class GetTotalQueryHandler(
    IInvestmentRepository investmentRepository,
    IExpenseRepository expenseRepository,
    IIncomeRepository incomeRepository,
    UserManager<User> userManager,
    IAuthService authService)
    : IQueryHandler<GetTotalQuery, List<TransactionBaseDto>>
{
    public async Task<List<TransactionBaseDto>> Handle(GetTotalQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(authService.GetSessionUser());
        if (user == null) throw new UnauthorizedAccessException("User not authenticated");
        
        var incomes = await incomeRepository.GetAll(
            user.Id,
            request.StartDate,
            request.EndDate,
            request.CategoryIds,
            cancellationToken);
        
        var expenses = await expenseRepository.GetAll(
            user.Id,
            request.StartDate,
            request.EndDate,
            request.CategoryIds,
            cancellationToken);
        
        var investments = await investmentRepository.GetAll(
            user.Id,
            request.StartDate,
            request.EndDate,
            request.CategoryIds,
            cancellationToken);
    
        var transactions = new List<TransactionBaseDto>();
     
        transactions.AddRange(incomes.Select(income => new TransactionBaseDto 
        {
            Id = income.Id,
            Amount = income.Amount,
            WalletName = income.Wallet.Name,
            UserId = income.UserId,
            WalletId = income.WalletId,
            CategoryName = income.Category.Name,
            Type = "Income",
            Description = income.Description,
            CategoryId = income.CategoryId,
            Date = income.CreatedAt
        }));

        transactions.AddRange(expenses.Select(expense => new TransactionBaseDto 
        {
            Id = expense.Id,
            Amount = expense.Amount,
            WalletName = expense.Wallet.Name,
            UserId = expense.UserId,
            WalletId = expense.WalletId,
            CategoryName = expense.Category.Name,
            Type = "Expense",
            Description = expense.Description,
            CategoryId = expense.CategoryId,
            Date = expense.CreatedAt
        }));

        transactions.AddRange(investments.Select(investment => new TransactionBaseDto 
        {
            Id = investment.Id,
            Amount = investment.Amount,
            WalletName = investment.Wallet.Name,
            UserId = investment.UserId,
            WalletId = investment.WalletId,
            CategoryName = investment.Category.Name,
            Type = "Investment",
            Description = investment.Description,
            CategoryId = investment.CategoryId,
            Date = investment.CreatedAt
        }));

        return transactions.OrderByDescending(t => t.Date).ToList();
    }
}