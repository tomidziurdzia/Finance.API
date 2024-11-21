using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Investment;

namespace FinanceApp.Application.Features.Investments.Queries.GetAll;

public class GetInvestmentsQuery : IQuery<InvestmentsDto>
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public List<Guid>? CategoryIds { get; set; }
}