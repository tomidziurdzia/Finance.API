namespace FinanceApp.Application.DTOs.InvestmentAccount;

public class TransferToInvestmentAccountDto
{
    public Guid WalletId { get; set; }
    public Guid InvestmentAccountId { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
}