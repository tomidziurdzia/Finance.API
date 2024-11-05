using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Income;
using FinanceApp.Application.Exceptions;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Incomes.Commands.CreateIncome;

public class CreateIncomeCommandHandler(
    IIncomeRepository incomeRepository,
    IWalletRepository walletRepository,
    ICategoryRepository categoryRepository,
    UserManager<User> userManager,
    IAuthService authService)
    : ICommandHandler<CreateIncomeCommand, IncomeDto>
{
    public async Task<IncomeDto> Handle(CreateIncomeCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(authService.GetSessionUser());
        if (user == null) throw new UnauthorizedAccessException("User not authenticated");

        var wallet = await walletRepository.Get(user.Id, request.WalletId, cancellationToken);
        if (wallet == null || wallet.UserId != user.Id)
        {
            throw new NotFoundException(nameof(wallet), request.WalletId);
        }

        Category? category = null;
        if (request.CategoryId.HasValue)
        {
            category = await categoryRepository.Get(user.Id, request.CategoryId.Value, cancellationToken);
            if (category == null)
            {
                throw new NotFoundException(nameof(category), request.CategoryId.Value);
            }
        }

        var income = new Income
        {
            WalletId = wallet.Id,
            CategoryId = category!.Id,
            UserId = user.Id,
            Amount = request.Amount,
            Description = request.Description,
            CreatedAt = request.Date
        };
        
        await incomeRepository.Create(income, cancellationToken);

        return new IncomeDto
        {
            Id = income.Id,
            WalletId = income.WalletId,
            WalletName = wallet.Name,
            CategoryId = income.CategoryId,
            CategoryName = category?.Name,
            UserId = income.UserId,
            Amount = income.Amount,
            Description = income.Description,
            Date = income.CreatedAt
        };
    }
}
