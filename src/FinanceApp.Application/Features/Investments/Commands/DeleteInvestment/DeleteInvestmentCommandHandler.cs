using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.Exceptions;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Investments.Commands.DeleteInvestment;

public class DeleteInvestmentCommandHandler(
    IInvestmentRepository investmentRepository,
    UserManager<User> userManager,
    IAuthService authService)
    : ICommandHandler<DeleteInvestmentCommand, Unit>
{
    public async Task<Unit> Handle(DeleteInvestmentCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(authService.GetSessionUser());
        if (user == null) throw new UnauthorizedAccessException("User not authenticated");

        var investment = await investmentRepository.Get(user.Id, request.Id, cancellationToken);
        if (investment == null || investment.UserId != user.Id)
        {
            throw new NotFoundException(nameof(Investment), request.Id);
        }

        await investmentRepository.Delete(investment, cancellationToken);

        return Unit.Value;
    }
}