using FinanceApp.Application.DTOs.Expense;
using FinanceApp.Application.DTOs.Income;
using FinanceApp.Application.DTOs.Investment;

namespace FinanceApp.Application.DTOs.Wallet;

public class WalletDto
{
    public Guid? Id { get; set; }
    public string Name { get; set; }
    public string Currency { get; set; }
    public List<IncomeDto> Income { get; set; } = new List<IncomeDto>();
    public List<ExpenseDto> Expense { get; set; } = new List<ExpenseDto>();
    public List<InvestmentDto> Investment { get; set; } = new List<InvestmentDto>();

}