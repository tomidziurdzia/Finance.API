namespace FinanceApp.Application.DTOs.Wallet;

public class TransactionWalletDto
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public DateTime? Date { get; set; }
    public string Type { get; set; }
}