using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.DTOs.InvestmentAccount;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using MediatR;

namespace FinanceApp.Application.Features.InvestmentAccounts.Commands.CreateInvestmentAccount;

public class CreateInvestmentAccountCommandHandler(
    IInvestmentAccountRepository investmentAccountRepository,
    UserManager<User> userManager,
    IAuthService authService)
    : IRequestHandler<CreateInvestmentAccountCommand, InvestmentAccountDto>
{
    public async Task<InvestmentAccountDto> Handle(CreateInvestmentAccountCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(authService.GetSessionUser());
        if (user == null) throw new UnauthorizedAccessException("User not authenticated");

        var investmentAccount = new InvestmentAccount
        {
            Name = request.Name,
            Currency = request.Currency,
            UserId = user.Id
        };

        await investmentAccountRepository.Create(investmentAccount, cancellationToken);

        return new InvestmentAccountDto
        {
            Id = investmentAccount.Id,
            Name = investmentAccount.Name,
            Currency = investmentAccount.Currency.ToString(),
            TotalBalance = 0
        };
    }
}