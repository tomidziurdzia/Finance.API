namespace FinanceApp.Application.DTOs.Investment;

public class InvestmentsDto
{
    public List<InvestmentDto> Data { get; set; } = [];
    public decimal Total { get; set; }
}