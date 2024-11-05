using FinanceApp.Application.CQRS;
using MediatR;

namespace FinanceApp.Application.Features.Incomes.Commands.DeleteIncome;

public class DeleteIncomeCommand : ICommand<Unit>
{
    public Guid Id { get; set; }

    public DeleteIncomeCommand(Guid id)
    {
        Id = id;
    }
}