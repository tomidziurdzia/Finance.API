using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.InvestmentAccounts.Commands.DeleteInvestmentAccount;

public class DeleteInvestmentAccountCommandHandler(
    IInvestmentAccountRepository investmentAccountRepository,
    UserManager<User> userManager,
    IAuthService authService)
    : ICommandHandler<DeleteInvestmentAccountCommand, Unit>
{
    public async Task<Unit> Handle(DeleteInvestmentAccountCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(authService.GetSessionUser());
        if (user == null) throw new UnauthorizedAccessException("User not authenticated");

        var investmentAccount = await investmentAccountRepository.Get(user.Id, request.Id, cancellationToken);
        if (investmentAccount == null || investmentAccount.UserId != user.Id)
        {
            throw new UnauthorizedAccessException("You do not have access to this investment account.");
        }

        await investmentAccountRepository.Delete(investmentAccount, cancellationToken);

        return Unit.Value;
    }
}