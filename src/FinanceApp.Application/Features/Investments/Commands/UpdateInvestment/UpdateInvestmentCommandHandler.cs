using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Investment;
using FinanceApp.Application.Exceptions;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Investments.Commands.UpdateInvestment;

public class UpdateInvestmentCommandHandler(
    IInvestmentRepository investmentRepository,
    IWalletRepository walletRepository,
    ICategoryRepository categoryRepository,
    UserManager<User> userManager,
    IAuthService authService)
    : ICommandHandler<UpdateInvestmentCommand, InvestmentDto>
{
    public async Task<InvestmentDto> Handle(UpdateInvestmentCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(authService.GetSessionUser());
        if (user == null) throw new UnauthorizedAccessException("User not authenticated");

        var investment = await investmentRepository.Get(user.Id, request.Id, cancellationToken);
        if (investment == null || investment.UserId != user.Id)
        {
            throw new NotFoundException(nameof(Investment), request.Id);
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
        var totalBalance = totalIncome - totalExpense;

        if (totalBalance < request.Amount)
        {
            throw new InvalidOperationException("Insufficient balance to create this investment.");
        }

        investment.WalletId = wallet.Id;
        investment.CategoryId = category.Id;
        investment.Amount = request.Amount;
        investment.Description = request.Description;

        await investmentRepository.Update(investment, cancellationToken);

        return new InvestmentDto
        {
            Id = investment.Id,
            WalletId = investment.WalletId,
            WalletName = wallet.Name,
            CategoryId = investment.CategoryId,
            CategoryName = category.Name!,
            UserId = investment.UserId,
            Amount = investment.Amount,
            Description = investment.Description,
            Date = investment.CreatedAt,
        };
    }
}
