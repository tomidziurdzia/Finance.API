using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Expense;

namespace FinanceApp.Application.Features.Expenses.Queries.GetAll;

public class GetExpensesQuery : IQuery<List<ExpenseDto>>
{
}