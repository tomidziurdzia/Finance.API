using FinanceApp.Application.DTOs.Expense;
using FinanceApp.Application.DTOs.Income;
using FinanceApp.Application.DTOs.Investment;
using FinanceApp.Application.DTOs.Transaction;

namespace FinanceApp.Application.DTOs.Overview;

public class OverviewDto
{
    public List<TransactionBaseDto> Transactions { get; set; } = [];
}