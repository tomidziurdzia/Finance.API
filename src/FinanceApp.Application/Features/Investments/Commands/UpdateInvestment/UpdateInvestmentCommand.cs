using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Investment;

namespace FinanceApp.Application.Features.Investments.Commands.UpdateInvestment;

public class UpdateInvestmentCommand : ICommand<InvestmentDto>
{
    public Guid Id { get; set; }
    public Guid WalletId { get; set; }
    public Guid CategoryId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
}