using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Category;
using FinanceApp.Domain.Models.Enums;

namespace FinanceApp.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommand : ICommand<CategoryDto>
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public CategoryType Type { get; set; }
}