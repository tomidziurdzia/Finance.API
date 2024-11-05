using FinanceApp.Application.CQRS;
using MediatR;

namespace FinanceApp.Application.Features.Investments.Commands.DeleteInvestment;

public class DeleteInvestmentCommand : ICommand<Unit>
{
    public Guid Id { get; set; }

    public DeleteInvestmentCommand(Guid id)
    {
        Id = id;
    }
}