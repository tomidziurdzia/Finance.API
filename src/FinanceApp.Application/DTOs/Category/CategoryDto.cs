
namespace FinanceApp.Application.DTOs.Category;

public class CategoryDto
{
    public Guid? Id { get; set; }
    public string Name { get; set; }
    public string ParentType { get; set; }
    public string Type { get; set; }
    public decimal Total { get; set; }
}