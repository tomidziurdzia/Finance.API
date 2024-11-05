using FinanceApp.Application.DTOs.Transaction;

namespace FinanceApp.Application.DTOs.Income;

public class IncomeDto : TransactionBaseDto
{
    public IncomeDto()
    {
        Type = "Income";
    }
}