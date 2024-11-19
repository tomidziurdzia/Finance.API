using FinanceApp.Domain.Models.Enums;

namespace FinanceApp.Application.DTOs.InvestmentAccount;

public class UpdateInvestmentAccountDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Currency Currency { get; set; }
}