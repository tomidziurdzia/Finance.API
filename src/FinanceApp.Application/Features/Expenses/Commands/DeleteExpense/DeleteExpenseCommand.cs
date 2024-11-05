using FinanceApp.Application.CQRS;
using MediatR;

namespace FinanceApp.Application.Features.Expenses.Commands.DeleteExpense;

public class DeleteExpenseCommand : ICommand<Unit>
{
    public Guid Id { get; set; }

    public DeleteExpenseCommand(Guid id)
    {
        Id = id;
    }
}