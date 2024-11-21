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
    : IQueryHandler<GetIncomesQuery, IncomesDto>
{
    public async Task<IncomesDto> Handle(GetIncomesQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(authService.GetSessionUser());
        if (user == null) throw new UnauthorizedAccessException("User not authenticated");

        var incomes = await incomeRepository.GetAll(
            user.Id,
            request.StartDate,
            request.EndDate,
            request.CategoryIds,
            cancellationToken);

        var incomeDtos = incomes
            .OrderByDescending(income => income.CreatedAt)
            .Select(income => new IncomeDto
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
                Date = income.CreatedAt,
            }).ToList();

        var total = incomeDtos
            .Where(income => income.CategoryName != "Transfer")
            .Sum(income => income.Amount);

        return new IncomesDto
        {
            Data = incomeDtos,
            Total = total
        };
    }

}