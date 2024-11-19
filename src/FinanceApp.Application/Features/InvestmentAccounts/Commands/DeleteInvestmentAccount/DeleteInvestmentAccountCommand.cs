using FinanceApp.Application.CQRS;
using MediatR;

namespace FinanceApp.Application.Features.InvestmentAccounts.Commands.DeleteInvestmentAccount;

public class DeleteInvestmentAccountCommand : ICommand<Unit>
{
    public Guid Id { get; set; }

    public DeleteInvestmentAccountCommand(Guid id)
    {
        Id = id;
    }
}