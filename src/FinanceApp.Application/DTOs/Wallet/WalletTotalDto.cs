namespace FinanceApp.Application.DTOs.Wallet;

public class WalletTotalDto
{
    public Guid Id { get; set; }
    public decimal Total { get; set; }
    public decimal Income { get; set; }
    public decimal Expense { get; set; }
    public decimal Investment { get; set; }
}

public class WalletTotalsResponseDto
{
    public List<WalletTotalDto> Wallets { get; set; } = new List<WalletTotalDto>();
    public decimal Total { get; set; }
    public decimal Income { get; set; }
    public decimal Expense { get; set; }
    public decimal Investment { get; set; }
}