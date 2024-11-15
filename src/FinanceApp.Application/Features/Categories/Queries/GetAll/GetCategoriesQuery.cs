using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Category;

namespace FinanceApp.Application.Features.Categories.Queries.GetAll;

public class GetCategoriesQuery : IQuery<List<CategoryDto>>
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public List<Guid>? CategoryIds { get; set; }
}