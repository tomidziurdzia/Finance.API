using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.Exceptions;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Transactions.Commands.DeleteTransaction;

public class DeleteTransactionCommandHandler(
    ITransactionRepository transactionRepository,
    UserManager<User> userManager,
    IAuthService authService)
    : ICommandHandler<DeleteTransactionCommand, Unit>
{
    public async Task<Unit> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(authService.GetSessionUser());
        if (user == null) throw new UnauthorizedAccessException("User not authenticated");

        var transaction = await transactionRepository.Get(user.Id, request.Id, cancellationToken);
        if (transaction == null || transaction.UserId != user.Id)
        {
            throw new NotFoundException(nameof(Transaction), request.Id);
        }

        await transactionRepository.Delete(transaction, cancellationToken);

        return Unit.Value;
    }
}