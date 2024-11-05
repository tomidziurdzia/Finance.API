using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.Exceptions;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Expenses.Commands.DeleteExpense;

public class DeleteExpenseCommandHandler(
    IExpenseRepository expenseRepository,
    UserManager<User> userManager,
    IAuthService authService)
    : ICommandHandler<DeleteExpenseCommand, Unit>
{
    public async Task<Unit> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(authService.GetSessionUser());
        if (user == null) throw new UnauthorizedAccessException("User not authenticated");

        var expense = await expenseRepository.Get(user.Id, request.Id, cancellationToken);
        if (expense == null || expense.UserId != user.Id)
        {
            throw new NotFoundException(nameof(Expense), request.Id);
        }

        await expenseRepository.Delete(expense, cancellationToken);

        return Unit.Value;
    }
}