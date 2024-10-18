using FinanceApp.Application.DTOs.Transaction;

namespace FinanceApp.Application.DTOs.Wallet;

public class WalletDto
{
    public Guid? Id { get; set; }
    public string Name { get; set; }
    public string Currency { get; set; }
    public List<WalletTransactionsDto> Transactions { get; set; } = new List<WalletTransactionsDto>();
    public decimal Total { get; set; }
}