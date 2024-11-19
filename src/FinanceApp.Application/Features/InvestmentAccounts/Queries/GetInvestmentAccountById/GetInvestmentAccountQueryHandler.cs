using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.InvestmentAccount;
using FinanceApp.Application.Exceptions;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.InvestmentAccounts.Queries.GetInvestmentAccountById;

public class GetInvestmentAccountQueryHandler(
    IInvestmentAccountRepository investmentAccountRepository,
    UserManager<User> userManager,
    IAuthService authService)
    : IQueryHandler<GetInvestmentAccountQuery, InvestmentAccountDto>
{
    public async Task<InvestmentAccountDto> Handle(GetInvestmentAccountQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(authService.GetSessionUser());
        if (user == null)
        {
            throw new UnauthorizedAccessException("User not authenticated");
        }

        var investmentAccount = await investmentAccountRepository.Get(user.Id, request.InvestmentAccountId, cancellationToken);
        if (investmentAccount == null)
        {
            throw new NotFoundException(nameof(InvestmentAccount), request.InvestmentAccountId);
        }

        if (investmentAccount.UserId != user.Id)
        {
            throw new UnauthorizedAccessException("You do not have access to this investment account.");
        }

        var totalBalance = investmentAccount.Investments.Sum(inv => inv.Amount);

        return new InvestmentAccountDto
        {
            Id = investmentAccount.Id,
            Name = investmentAccount.Name,
            Currency = investmentAccount.Currency.ToString(),
            TotalBalance = totalBalance,
            Investments = investmentAccount.Investments
                .Select(investment => new InvestmentTransactionDto
                {
                    Id = investment.Id,
                    CategoryId = investment.CategoryId,
                    CategoryName = investment.Category?.Name ?? "Unknown",
                    Amount = investment.Amount,
                    Description = investment.Description,
                    Date = investment.CreatedAt
                })
                .OrderByDescending(i => i.Date)
                .ToList()
        };
    }
}
