using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Income;

namespace FinanceApp.Application.Features.Incomes.Queries.GetAll;

public class GetIncomesQuery : IQuery<List<IncomeDto>>
{
}