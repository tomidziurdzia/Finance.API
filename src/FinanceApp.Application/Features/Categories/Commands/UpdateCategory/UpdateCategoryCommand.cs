using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Category;

namespace FinanceApp.Application.Features.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommand : ICommand<CategoryDto>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; } 
}