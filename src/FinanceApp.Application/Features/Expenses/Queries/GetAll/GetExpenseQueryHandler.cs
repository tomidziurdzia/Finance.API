using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Expense;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Expenses.Queries.GetAll;

public class GetExpensesQueryHandler(
    IExpenseRepository expenseRepository,
    UserManager<User> userManager,
    IAuthService authService)
    : IQueryHandler<GetExpensesQuery, ExpensesDto>
{
    public async Task<ExpensesDto> Handle(GetExpensesQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(authService.GetSessionUser());
        if (user == null) throw new UnauthorizedAccessException("User not authenticated");

        var expenses = await expenseRepository.GetAll(
            user.Id,
            request.StartDate,
            request.EndDate,
            request.CategoryIds,
            cancellationToken);

        var filteredExpenses = expenses
            .Where(expense => expense.Category.Name != "Transfer")
            .OrderByDescending(expense => expense.CreatedAt);

        var expenseDtos = filteredExpenses
            .Select(expense => new ExpenseDto
            {
                Id = expense.Id,
                WalletId = expense.WalletId,
                WalletName = expense.Wallet?.Name,
                InvestmentAccountId = expense.InvestmentAccountId,
                InvestmentAccountName = expense.InvestmentAccount?.Name,
                CategoryId = expense.CategoryId,
                CategoryName = expense.Category.Name,
                UserId = expense.UserId,
                Amount = expense.Amount,
                Description = expense.Description,
                Date = expense.CreatedAt
            }).ToList();

        var total = expenseDtos
            .Where(expense => expense.CategoryName != "Transfer")
            .Sum(expense => expense.Amount);

        return new ExpensesDto
        {
            Data = expenseDtos,
            Total = total
        };
    }
}