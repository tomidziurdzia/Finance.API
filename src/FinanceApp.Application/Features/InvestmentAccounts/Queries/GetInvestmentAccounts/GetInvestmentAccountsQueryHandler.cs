using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.InvestmentAccount;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.InvestmentAccounts.Queries.GetInvestmentAccounts;

public class GetInvestmentAccountsQueryHandler(
    IInvestmentAccountRepository investmentAccountRepository,
    UserManager<User> userManager,
    IAuthService authService)
    : IQueryHandler<GetInvestmentAccountsQuery, List<InvestmentAccountDto>>
{
    public async Task<List<InvestmentAccountDto>> Handle(GetInvestmentAccountsQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(authService.GetSessionUser());
        if (user == null) 
            throw new UnauthorizedAccessException("User not authenticated");

        var investmentAccounts = await investmentAccountRepository.GetAll(user.Id, cancellationToken);

        return investmentAccounts.Select(account => new InvestmentAccountDto
        {
            Id = account.Id,
            Name = account.Name,
            Currency = account.Currency.ToString(),
            TotalBalance = account.Investments.Sum(inv => inv.Amount)
                           + account.Income.Sum(inc => inc.Amount)
                           - account.Expense.Sum(exp => exp.Amount),
            Investments = account.Investments.Select(investment => new InvestmentTransactionDto
            {
                Id = investment.Id,
                CategoryId = investment.CategoryId,
                CategoryName = investment.Category?.Name,
                Amount = investment.Amount,
                Description = investment.Description,
                Date = investment.CreatedAt ?? DateTime.MinValue
            }).ToList()
        }).ToList();
    }
}
