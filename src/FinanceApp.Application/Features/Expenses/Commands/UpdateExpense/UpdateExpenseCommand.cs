using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Expense;

namespace FinanceApp.Application.Features.Expenses.Commands.UpdateExpense;

public class UpdateExpenseCommand : ICommand<ExpenseDto>
{
    public Guid Id { get; set; }
    public Guid WalletId { get; set; }
    public Guid CategoryId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
}