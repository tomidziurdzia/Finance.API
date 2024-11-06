using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Income;

namespace FinanceApp.Application.Features.Incomes.Queries.GetAll;

public class GetIncomesQuery : IQuery<List<IncomeDto>>
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public List<Guid>? CategoryIds { get; set; }
}