using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Income;

namespace FinanceApp.Application.Features.Incomes.Commands.UpdateIncome;

public class UpdateIncomeCommand : ICommand<IncomeDto>
{
    public Guid Id { get; set; }
    public Guid WalletId { get; set; }
    public Guid CategoryId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
}