using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Income;

namespace FinanceApp.Application.Features.Incomes.Commands.CreateIncome;

public class CreateIncomeCommand : ICommand<IncomeDto>
{
    public Guid WalletId { get; set; }
    public Guid? CategoryId { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }
}