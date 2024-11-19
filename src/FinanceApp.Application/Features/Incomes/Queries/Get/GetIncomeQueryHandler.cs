using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Income;
using FinanceApp.Application.Exceptions;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Incomes.Queries.Get;

public class GetIncomeQueryHandler(
    IIncomeRepository incomeRepository,
    UserManager<User> userManager,
    IAuthService authService
) : IQueryHandler<GetIncomeQuery, IncomeDto>
{
    public async Task<IncomeDto> Handle(GetIncomeQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(authService.GetSessionUser());
        if (user == null) throw new UnauthorizedAccessException("User not authenticated");

        var income = await incomeRepository.Get(user.Id, request.IncomeId, cancellationToken);
        if (income == null || income.UserId != user.Id)
        {
            throw new NotFoundException(nameof(Income), request.IncomeId);
        }

        return new IncomeDto
        {
            Id = income.Id,
            WalletId = income.WalletId,
            WalletName = income.Wallet?.Name,
            InvestmentAccountId = income.InvestmentAccountId,
            InvestmentAccountName = income.InvestmentAccount?.Name,
            CategoryId = income.CategoryId,
            CategoryName = income.Category.Name,
            UserId = income.UserId,
            Amount = income.Amount,
            Description = income.Description,
            Date = income.CreatedAt
        };
    }
}