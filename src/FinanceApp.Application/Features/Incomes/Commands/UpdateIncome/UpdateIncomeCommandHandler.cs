using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Income;
using FinanceApp.Application.Exceptions;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Incomes.Commands.UpdateIncome;

public class UpdateIncomeCommandHandler(
    IIncomeRepository incomeRepository,
    IWalletRepository walletRepository,
    ICategoryRepository categoryRepository,
    UserManager<User> userManager,
    IAuthService authService)
    : ICommandHandler<UpdateIncomeCommand, IncomeDto>
{
    public async Task<IncomeDto> Handle(UpdateIncomeCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(authService.GetSessionUser());
        if (user == null) throw new UnauthorizedAccessException("User not authenticated");

        var income = await incomeRepository.Get(user.Id, request.Id, cancellationToken);
        if (income == null || income.UserId != user.Id)
        {
            throw new NotFoundException(nameof(Income), request.Id);
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

        income.WalletId = wallet.Id;
        income.CategoryId = category.Id;
        income.Amount = request.Amount;
        income.Description = request.Description;

        await incomeRepository.Update(income, cancellationToken);

        return new IncomeDto
        {
            Id = income.Id,
            WalletId = income.WalletId,
            WalletName = wallet.Name,
            CategoryId = income.CategoryId,
            CategoryName = category.Name!,
            UserId = income.UserId,
            Amount = income.Amount,
            Description = income.Description,
            Date = income.CreatedAt,
        };
    }
}
