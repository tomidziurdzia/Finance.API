using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Investment;
using FinanceApp.Application.Exceptions;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Investments.Queries.Get;

public class GetInvestmentQueryHandler(
    IInvestmentRepository investmentRepository,
    UserManager<User> userManager,
    IAuthService authService
) : IQueryHandler<GetInvestmentQuery, InvestmentDto>
{
    public async Task<InvestmentDto> Handle(GetInvestmentQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(authService.GetSessionUser());
        if (user == null) throw new UnauthorizedAccessException("User not authenticated");

        var investment = await investmentRepository.Get(user.Id, request.InvestmentId, cancellationToken);
        if (investment == null || investment.UserId != user.Id)
        {
            throw new NotFoundException(nameof(Investment), request.InvestmentId);
        }

        return new InvestmentDto
        {
            Id = investment.Id,
            WalletId = investment.WalletId,
            WalletName = investment.Wallet?.Name,
            InvestmentAccountId = investment.InvestmentAccountId,
            InvestmentAccountName = investment.InvestmentAccount?.Name,
            CategoryId = investment.CategoryId,
            CategoryName = investment.Category.Name,
            UserId = investment.UserId,
            Amount = investment.Amount,
            Description = investment.Description,
            Date = investment.CreatedAt
        };
    }
}