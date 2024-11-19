namespace FinanceApp.Application.DTOs.InvestmentAccount;

public class InvestmentTransactionDto
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public DateTime? Date { get; set; }
}