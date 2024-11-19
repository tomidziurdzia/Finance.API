using FinanceApp.Domain.Models.Enums;

namespace FinanceApp.Application.DTOs.InvestmentAccount;

public class CreateInvestmentAccountDto
{
    public string Name { get; set; }
    public Currency Currency { get; set; }
}