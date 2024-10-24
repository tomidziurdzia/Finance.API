using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Category;

namespace FinanceApp.Application.Features.Categories.Queries.GetById;

public class GetCategoryQuery(Guid categoryId) : IQuery<CategoryDto>
{
    public Guid CategoryId { get; set; } = categoryId;
}