using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.InvestmentAccount;
using FinanceApp.Application.Exceptions;
using FinanceApp.Application.Features.Wallets.Commands.UpdateWallet;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.InvestmentAccounts.Commands.UpdateInvestmentAccount;

public class UpdateInvestmentAccountCommandHandler(
    IInvestmentAccountRepository investmentAccountRepository,
    UserManager<User> userManager,
    IAuthService authService)
    : ICommandHandler<UpdateInvestmentAccountCommand, InvestmentAccountDto>
{
    public async Task<InvestmentAccountDto> Handle(UpdateInvestmentAccountCommand request, CancellationToken cancellationToken)
    {
        // Obtener el usuario autenticado
        var user = await userManager.FindByNameAsync(authService.GetSessionUser());
        if (user == null) throw new UnauthorizedAccessException("User not authenticated");

        // Obtener la cuenta de inversión
        var investmentAccount = await investmentAccountRepository.Get(user.Id, request.Id, cancellationToken);
        if (investmentAccount == null)
        {
            throw new NotFoundException(request.Name,$"InvestmentAccount with ID {request.Id} not found.");
        }

        if (investmentAccount.UserId != user.Id)
        {
            throw new UnauthorizedAccessException("You do not have access to this investment account.");
        }

        investmentAccount.Name = request.Name;

        await investmentAccountRepository.Update(investmentAccount, cancellationToken);

        return new InvestmentAccountDto
        {
            Id = investmentAccount.Id,
            Name = investmentAccount.Name,
            Currency = investmentAccount.Currency.ToString(),
        };
    }
}
