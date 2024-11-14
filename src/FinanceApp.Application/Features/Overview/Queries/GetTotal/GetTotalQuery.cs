using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Transaction;

namespace FinanceApp.Application.Features.Overview.Queries.GetTotal;

public class GetTotalQuery : IQuery<List<TransactionBaseDto>>
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public List<Guid>? CategoryIds { get; set; }
}