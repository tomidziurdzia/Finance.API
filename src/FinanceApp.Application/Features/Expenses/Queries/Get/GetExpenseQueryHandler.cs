using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Expense;
using FinanceApp.Application.Exceptions;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Expenses.Queries.Get;

public class GetExpenseQueryHandler(
    IExpenseRepository expenseRepository,
    UserManager<User> userManager,
    IAuthService authService
) : IQueryHandler<GetExpenseQuery, ExpenseDto>
{
    public async Task<ExpenseDto> Handle(GetExpenseQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(authService.GetSessionUser());
        if (user == null) throw new UnauthorizedAccessException("User not authenticated");

        var expense = await expenseRepository.Get(user.Id, request.ExpenseId, cancellationToken);
        if (expense == null || expense.UserId != user.Id)
        {
            throw new NotFoundException(nameof(Expense), request.ExpenseId);
        }

        return new ExpenseDto
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
        };
    }
}