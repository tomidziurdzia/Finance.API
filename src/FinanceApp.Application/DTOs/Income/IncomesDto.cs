namespace FinanceApp.Application.DTOs.Income;

public class IncomesDto
{
    public List<IncomeDto> Data { get; set; } = [];
    public decimal Total { get; set; }
}