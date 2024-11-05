using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Expense;

namespace FinanceApp.Application.Features.Expenses.Queries.Get;

public class GetExpenseQuery : IQuery<ExpenseDto>
{
    public Guid ExpenseId { get; set; }

    public GetExpenseQuery(Guid expenseId)
    {
        ExpenseId = expenseId;
    }
}