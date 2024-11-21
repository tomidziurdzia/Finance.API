using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Expense;

namespace FinanceApp.Application.Features.Expenses.Queries.GetAll;

public class GetExpensesQuery : IQuery<ExpensesDto>
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public List<Guid>? CategoryIds { get; set; }
}