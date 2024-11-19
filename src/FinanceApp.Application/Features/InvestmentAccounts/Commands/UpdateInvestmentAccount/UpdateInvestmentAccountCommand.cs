using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.InvestmentAccount;

namespace FinanceApp.Application.Features.InvestmentAccounts.Commands.UpdateInvestmentAccount;

public class UpdateInvestmentAccountCommand : ICommand<InvestmentAccountDto>
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public UpdateInvestmentAccountCommand(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}