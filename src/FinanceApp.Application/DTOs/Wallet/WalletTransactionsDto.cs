namespace FinanceApp.Application.DTOs.Wallet;

public class WalletTransactionsDto
{
    public Guid Id { get; set; }
    public string CategoryName { get; set; }
    public string Type { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public DateTime? CreatedAt { get; set; }
}