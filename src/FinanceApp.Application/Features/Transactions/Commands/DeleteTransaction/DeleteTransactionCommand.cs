using FinanceApp.Application.CQRS;
using MediatR;

namespace FinanceApp.Application.Features.Transactions.Commands.DeleteTransaction;

public class DeleteTransactionCommand : ICommand<Unit>
{
    public Guid Id { get; set; }

    public DeleteTransactionCommand(Guid id)
    {
        Id = id;
    }
}