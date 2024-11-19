using FinanceApp.Application.DTOs.Expense;
using FinanceApp.Application.DTOs.Income;

namespace FinanceApp.Application.DTOs.InvestmentAccount;

public class InvestmentAccountDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Currency { get; set; }
    public decimal TotalBalance { get; set; }
    public List<InvestmentTransactionDto> Investments { get; set; } = new();
}