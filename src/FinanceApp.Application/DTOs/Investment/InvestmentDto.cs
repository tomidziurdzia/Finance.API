using FinanceApp.Application.DTOs.Transaction;

namespace FinanceApp.Application.DTOs.Investment;

public class InvestmentDto : TransactionBaseDto
{
    public InvestmentDto()
    {
        Type = "Investment";
    }
}