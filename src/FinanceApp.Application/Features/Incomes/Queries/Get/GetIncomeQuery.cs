using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Income;

namespace FinanceApp.Application.Features.Incomes.Queries.Get;

public class GetIncomeQuery : IQuery<IncomeDto>
{
    public Guid IncomeId { get; set; }

    public GetIncomeQuery(Guid incomeId)
    {
        IncomeId = incomeId;
    }
}