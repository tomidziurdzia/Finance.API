namespace FinanceApp.Application.DTOs.Expense;

public class ExpensesDto
{
    public List<ExpenseDto> Data { get; set; } = [];
    public decimal Total { get; set; }
}