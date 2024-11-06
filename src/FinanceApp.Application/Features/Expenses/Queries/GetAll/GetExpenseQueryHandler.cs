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
    : IQueryHandler<GetExpensesQuery, List<ExpenseDto>>
{
    public async Task<List<ExpenseDto>> Handle(GetExpensesQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(authService.GetSessionUser());
        if (user == null) throw new UnauthorizedAccessException("User not authenticated");

        var expenses = await expenseRepository.GetAll(
            user.Id,
            request.StartDate,
            request.EndDate,
            request.CategoryIds,
            cancellationToken);

        return expenses
            .OrderByDescending(expense => expense.CreatedAt)
            .Select(expense => new ExpenseDto
            {
                Id = expense.Id,
                WalletId = expense.WalletId,
                WalletName = expense.Wallet.Name,
                CategoryId = expense.CategoryId,
                CategoryName = expense.Category.Name,
                UserId = expense.UserId,
                Amount = expense.Amount,
                Description = expense.Description,
                Date = expense.CreatedAt
            }).ToList();
    }
}