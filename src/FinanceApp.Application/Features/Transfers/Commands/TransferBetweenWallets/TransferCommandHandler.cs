using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.Exceptions;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Transfers.Commands.TransferBetweenWallets;

public class TransferCommandHandler(
    IWalletRepository walletRepository,
    IIncomeRepository incomeRepository,
    IExpenseRepository expenseRepository,
    ICategoryRepository categoryRepository,
    UserManager<User> userManager,
    IAuthService authService)
    : ICommandHandler<TransferCommand>
{
    public async Task<Unit> Handle(TransferCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(authService.GetSessionUser());
        if (user == null) throw new UnauthorizedAccessException("User not authenticated");

        var sourceWallet = await walletRepository.Get(user.Id, request.SourceWalletId, cancellationToken);
        var destinationWallet = await walletRepository.Get(user.Id, request.DestinationWalletId, cancellationToken);
        
        if (sourceWallet == null || sourceWallet.UserId != user.Id)
            throw new NotFoundException(nameof(Wallet), request.SourceWalletId);
        if (destinationWallet == null || destinationWallet.UserId != user.Id)
            throw new NotFoundException(nameof(Wallet), request.DestinationWalletId);
        
        var totalIncome = sourceWallet.Income.Sum(income => income.Amount);
        var totalExpense = sourceWallet.Expense.Sum(expense => expense.Amount);
        var balance = totalIncome - totalExpense;

        if (balance < request.Amount)
        {
            throw new BadRequestException("Insufficient funds for the transfer.");
        }

        var category = await categoryRepository.GetByName(user.Id, "Transfer", cancellationToken);

        var expense = new Expense
        {
            WalletId = sourceWallet.Id,
            UserId = user.Id,
            CategoryId = category.Id,
            Amount = request.Amount,
            Description = "Transfer from " + sourceWallet.Name,
        };

        var income = new Income
        {
            WalletId = destinationWallet.Id,
            UserId = user.Id,
            Amount = request.Amount,
            CategoryId = category.Id,
            Description = "Transfer to " + destinationWallet.Name,
        };
        
        await expenseRepository.Create(expense, cancellationToken);
        await incomeRepository.Create(income, cancellationToken);

        return Unit.Value;
    }
}
