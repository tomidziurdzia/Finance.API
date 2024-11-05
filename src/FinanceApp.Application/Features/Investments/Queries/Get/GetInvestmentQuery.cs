using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Investment;

namespace FinanceApp.Application.Features.Investments.Queries.Get;

public class GetInvestmentQuery : IQuery<InvestmentDto>
{
    public Guid InvestmentId { get; set; }

    public GetInvestmentQuery(Guid investmentId)
    {
        InvestmentId = investmentId;
    }
}