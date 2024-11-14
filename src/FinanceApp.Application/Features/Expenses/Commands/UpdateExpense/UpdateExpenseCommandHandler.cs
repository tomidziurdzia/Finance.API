using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Expense;
using FinanceApp.Application.Exceptions;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Expenses.Commands.UpdateExpense;

public class UpdateExpenseCommandHandler(
    IExpenseRepository expenseRepository,
    IWalletRepository walletRepository,
    ICategoryRepository categoryRepository,
    UserManager<User> userManager,
    IAuthService authService)
    : ICommandHandler<UpdateExpenseCommand, ExpenseDto>
{
    public async Task<ExpenseDto> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(authService.GetSessionUser());
        if (user == null) throw new UnauthorizedAccessException("User not authenticated");

        var expense = await expenseRepository.Get(user.Id, request.Id, cancellationToken);
        if (expense == null || expense.UserId != user.Id)
        {
            throw new NotFoundException(nameof(Expense), request.Id);
        }

        var wallet = await walletRepository.Get(user.Id, request.WalletId, cancellationToken);
        if (wallet == null || wallet.UserId != user.Id)
        {
            throw new NotFoundException(nameof(Wallet), request.WalletId);
        }

        var category = await categoryRepository.Get(user.Id, request.CategoryId, cancellationToken);
        if (category == null)
        {
            throw new NotFoundException(nameof(Category), request.CategoryId);
        }
        
        var totalIncome = wallet.Income.Sum(income => income.Amount);
        var totalExpense = wallet.Expense.Sum(expense => expense.Amount);
        var totalInvestment = wallet.Investment.Sum(investment => investment.Amount);
        var totalBalance = totalIncome - totalExpense - totalInvestment;

        if (totalBalance < request.Amount)
        {
            throw new InvalidOperationException("Insufficient balance to create this investment.");
        }

        expense.WalletId = wallet.Id;
        expense.CategoryId = category.Id;
        expense.Amount = request.Amount;
        expense.Description = request.Description;

        await expenseRepository.Update(expense, cancellationToken);

        return new ExpenseDto
        {
            Id = expense.Id,
            WalletId = expense.WalletId,
            WalletName = wallet.Name,
            CategoryId = expense.CategoryId,
            CategoryName = category.Name!,
            UserId = expense.UserId,
            Amount = expense.Amount,
            Description = expense.Description,
            Date = expense.CreatedAt,
        };
    }
}
