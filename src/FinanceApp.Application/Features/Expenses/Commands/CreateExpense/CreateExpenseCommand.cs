using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Expense;

namespace FinanceApp.Application.Features.Expenses.Commands.CreateExpense;

public class CreateExpenseCommand : ICommand<ExpenseDto>
{
    public Guid WalletId { get; set; }
    public Guid CategoryId { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }
}