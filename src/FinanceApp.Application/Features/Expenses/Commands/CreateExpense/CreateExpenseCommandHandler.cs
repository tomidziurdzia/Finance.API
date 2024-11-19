using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Expense;
using FinanceApp.Application.Exceptions;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Expenses.Commands.CreateExpense;

public class CreateExpenseCommandHandler(
    IExpenseRepository expenseRepository,
    IWalletRepository walletRepository,
    ICategoryRepository categoryRepository,
    UserManager<User> userManager,
    IAuthService authService)
    : ICommandHandler<CreateExpenseCommand, ExpenseDto>
{
    public async Task<ExpenseDto> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(authService.GetSessionUser());
        if (user == null) throw new UnauthorizedAccessException("User not authenticated");

        var wallet = await walletRepository.Get(user.Id, request.WalletId, cancellationToken);
        if (wallet == null || wallet.UserId != user.Id)
        {
            throw new NotFoundException(nameof(wallet), request.WalletId);
        }

        var category = await categoryRepository.Get(user.Id, request.CategoryId, cancellationToken);
        if (category == null)
        {
            throw new NotFoundException(nameof(category), request.CategoryId);
        }
        
        var totalIncome = wallet.Income.Sum(income => income.Amount);
        var totalExpense = wallet.Expense.Sum(expense => expense.Amount);
        var totalBalance = totalIncome - totalExpense;

        if (totalBalance < request.Amount)
        {
            throw new InvalidOperationException("Insufficient balance to create this expense.");
        }

        var expense = new Expense
        {
            WalletId = wallet.Id,
            CategoryId = category.Id,
            UserId = user.Id,
            Amount = request.Amount,
            Description = request.Description,
            CreatedAt = request.Date
        };
        
        await expenseRepository.Create(expense, cancellationToken);

        return new ExpenseDto
        {
            Id = expense.Id,
            WalletId = expense.WalletId,
            WalletName = wallet.Name,
            CategoryId = expense.CategoryId,
            CategoryName = category.Name,
            UserId = expense.UserId,
            Amount = expense.Amount,
            Description = expense.Description,
            Date = expense.CreatedAt
        };
    }
}