namespace FinanceApp.Application.DTOs.Wallet;

public class WalletDto
{
    public Guid? Id { get; set; }
    public string Name { get; set; }
    public string Currency { get; set; }
    public List<TransactionWalletDto> Transactions { get; set; } = new List<TransactionWalletDto>();
    public decimal TotalBalance { get; set; }
}