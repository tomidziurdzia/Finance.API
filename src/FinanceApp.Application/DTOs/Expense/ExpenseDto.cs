using FinanceApp.Application.DTOs.Transaction;

namespace FinanceApp.Application.DTOs.Expense;

public class ExpenseDto : TransactionBaseDto
{
    public ExpenseDto()
    {
        Type = "Expense";
    }
}