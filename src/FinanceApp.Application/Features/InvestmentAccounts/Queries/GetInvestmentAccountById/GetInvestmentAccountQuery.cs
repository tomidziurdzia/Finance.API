using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.InvestmentAccount;

namespace FinanceApp.Application.Features.InvestmentAccounts.Queries.GetInvestmentAccountById;

public class GetInvestmentAccountQuery : IQuery<InvestmentAccountDto>
{
    public Guid InvestmentAccountId { get; set; }

    public GetInvestmentAccountQuery(Guid investmentAccountId)
    {
        InvestmentAccountId = investmentAccountId;
    }
}