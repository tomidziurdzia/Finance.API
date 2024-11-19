using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.Exceptions;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Transfers.Commands.TransferWalletToInvestmentAccount;

public class TransferToInvestmentCommandHandler(
    IWalletRepository walletRepository,
    IInvestmentAccountRepository investmentAccountRepository,
    IExpenseRepository expenseRepository,
    IIncomeRepository incomeRepository,
    ICategoryRepository categoryRepository,
    UserManager<User> userManager,
    IAuthService authService)
    : ICommandHandler<TransferToInvestmentCommand>
{
    public async Task<Unit> Handle(TransferToInvestmentCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(authService.GetSessionUser());
        if (user == null) throw new UnauthorizedAccessException("User not authenticated");

        var sourceWallet = await walletRepository.Get(user.Id, request.SourceWalletId, cancellationToken);
        if (sourceWallet == null || sourceWallet.UserId != user.Id)
        {
            throw new NotFoundException(nameof(Wallet), request.SourceWalletId);
        }

        var investmentAccount = await investmentAccountRepository.Get(user.Id, request.InvestmentAccountId, cancellationToken);
        if (investmentAccount == null || investmentAccount.UserId != user.Id)
        {
            throw new NotFoundException(nameof(InvestmentAccount), request.InvestmentAccountId);
        }

        var totalIncome = sourceWallet.Income.Sum(income => income.Amount);
        var totalExpense = sourceWallet.Expense.Sum(expense => expense.Amount);
        var balance = totalIncome - totalExpense;
        
        if (balance < request.Amount)
        {
            throw new BadRequestException("Insufficient funds for the transfer.");
        }

        var category = await categoryRepository.GetByName(user.Id, "Transfer", cancellationToken);
        if (category == null)
        {
            throw new NotFoundException(nameof(Category), "Transfer");
        }

        var expense = new Expense
        {
            WalletId = sourceWallet.Id,
            UserId = user.Id,
            CategoryId = category.Id,
            Amount = request.Amount,
            Description = "Transfer to Investment Account " + investmentAccount.Name,
        };

        var income = new Income
        {
            InvestmentAccountId = investmentAccount.Id,
            UserId = user.Id,
            Amount = request.Amount,
            CategoryId = category.Id,
            Description = "Transfer from Wallet " + sourceWallet.Name,
        };

        await expenseRepository.Create(expense, cancellationToken);
        await incomeRepository.Create(income, cancellationToken);

        return Unit.Value;
    }
}