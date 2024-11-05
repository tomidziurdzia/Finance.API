using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.Exceptions;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Incomes.Commands.DeleteIncome;

public class DeleteIncomeCommandHandler(
    IIncomeRepository incomeRepository,
    UserManager<User> userManager,
    IAuthService authService)
    : ICommandHandler<DeleteIncomeCommand, Unit>
{
    public async Task<Unit> Handle(DeleteIncomeCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(authService.GetSessionUser());
        if (user == null) throw new UnauthorizedAccessException("User not authenticated");

        var income = await incomeRepository.Get(user.Id, request.Id, cancellationToken);
        if (income == null || income.UserId != user.Id)
        {
            throw new NotFoundException(nameof(Income), request.Id);
        }

        await incomeRepository.Delete(income, cancellationToken);

        return Unit.Value;
    }
}