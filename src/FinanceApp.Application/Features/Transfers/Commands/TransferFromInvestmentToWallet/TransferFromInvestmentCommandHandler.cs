using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.Exceptions;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Transfers.Commands.TransferFromInvestmentToWallet;

public class TransferFromInvestmentCommandHandler(
    IInvestmentAccountRepository investmentAccountRepository,
    IWalletRepository walletRepository,
    IExpenseRepository expenseRepository,
    IIncomeRepository incomeRepository,
    ICategoryRepository categoryRepository,
    UserManager<User> userManager,
    IAuthService authService)
    : ICommandHandler<TransferFromInvestmentCommand>
{
    public async Task<Unit> Handle(TransferFromInvestmentCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(authService.GetSessionUser());
        if (user == null)
        {
            throw new UnauthorizedAccessException("User not authenticated");
        }

        // Validar cuenta de inversión de origen
        var investmentAccount = await investmentAccountRepository.Get(user.Id, request.InvestmentAccountId, cancellationToken);
        if (investmentAccount == null || investmentAccount.UserId != user.Id)
        {
            throw new NotFoundException(nameof(InvestmentAccount), request.InvestmentAccountId);
        }

        // Validar wallet de destino
        var destinationWallet = await walletRepository.Get(user.Id, request.DestinationWalletId, cancellationToken);
        if (destinationWallet == null || destinationWallet.UserId != user.Id)
        {
            throw new NotFoundException(nameof(Wallet), request.DestinationWalletId);
        }

        // Calcular saldo disponible en la cuenta de inversión
        var totalIncome = investmentAccount.Income.Sum(inc => inc.Amount);
        var totalExpense = investmentAccount.Expense.Sum(exp => exp.Amount);
        var balance = totalIncome - totalExpense;

        if (balance < request.Amount)
        {
            throw new BadRequestException("Insufficient funds for the transfer.");
        }

        // Validar o crear categoría de transferencia
        var category = await categoryRepository.GetByName(user.Id, "Transfer", cancellationToken);
        if (category == null)
        {
            throw new NotFoundException(nameof(Category), "Transfer");
        }

        // Crear un Expense en la cuenta de inversión
        var expense = new Expense
        {
            InvestmentAccountId = investmentAccount.Id,
            UserId = user.Id,
            CategoryId = category.Id,
            Amount = request.Amount,
            Description = "Transfer to Wallet " + destinationWallet.Name,
        };

        // Crear un Income en la wallet de destino
        var income = new Income
        {
            WalletId = destinationWallet.Id,
            UserId = user.Id,
            Amount = request.Amount,
            CategoryId = category.Id,
            Description = "Transfer from Investment Account " + investmentAccount.Name,
        };

        // Registrar los movimientos
        await expenseRepository.Create(expense, cancellationToken);
        await incomeRepository.Create(income, cancellationToken);

        return Unit.Value;
    }
}
