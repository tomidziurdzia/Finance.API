namespace FinanceApp.Application.DTOs.Transaction;

public class TransactionDto
{
    public Guid Id { get; set; }
    public Guid WalletId { get; set; }
    public string WalletName { get; set; }
    public Guid? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public string UserId { get; set; }
    public string Type { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public DateTime? CreatedAt { get; set; }
}