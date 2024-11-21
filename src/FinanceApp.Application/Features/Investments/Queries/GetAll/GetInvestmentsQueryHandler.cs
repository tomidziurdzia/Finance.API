using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Investment;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Investments.Queries.GetAll;

public class GetInvestmentsQueryHandler(
    IInvestmentRepository investmentRepository,
    UserManager<User> userManager,
    IAuthService authService)
    : IQueryHandler<GetInvestmentsQuery, InvestmentsDto>
{
    public async Task<InvestmentsDto> Handle(GetInvestmentsQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(authService.GetSessionUser());
        if (user == null) throw new UnauthorizedAccessException("User not authenticated");
        
        var investments = await investmentRepository.GetAll(
            user.Id,
            request.StartDate,
            request.EndDate,
            request.CategoryIds,
            cancellationToken);

        var investmentDtos = investments
            .OrderByDescending(investment => investment.CreatedAt)
            .Select(investment => new InvestmentDto
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
            }).ToList();

        var total = investmentDtos
            .Where(investment => investment.CategoryName != "Transfer")
            .Sum(investment => investment.Amount);

        return new InvestmentsDto
        {
            Data = investmentDtos,
            Total = total
        };
    }
}