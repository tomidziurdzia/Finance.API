using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Investment;

namespace FinanceApp.Application.Features.Investments.Commands.CreateInvestment;

public class CreateInvestmentCommand : ICommand<InvestmentDto>
{
    public Guid WalletId { get; set; }
    public Guid? CategoryId { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }
}