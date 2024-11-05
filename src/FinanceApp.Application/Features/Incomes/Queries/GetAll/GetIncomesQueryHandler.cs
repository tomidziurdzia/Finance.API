using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Income;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Incomes.Queries.GetAll;

public class GetIncomesQueryHandler(
    IIncomeRepository incomeRepository,
    UserManager<User> userManager,
    IAuthService authService)
    : IQueryHandler<GetIncomesQuery, List<IncomeDto>>
{
    public async Task<List<IncomeDto>> Handle(GetIncomesQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(authService.GetSessionUser());
        if (user == null) throw new UnauthorizedAccessException("User not authenticated");

        var incomes = await incomeRepository.GetAll(user.Id, cancellationToken);
        
        return incomes
            .OrderByDescending(income => income.CreatedAt)
            .Select(income => new IncomeDto
            {
                Id = income.Id,
                WalletId = income.WalletId,
                WalletName = income.Wallet.Name,
                CategoryId = income.CategoryId,
                CategoryName = income.Category.Name,
                UserId = income.UserId,
                Amount = income.Amount,
                Description = income.Description,
                Date = income.CreatedAt
            }).ToList();
    }
}