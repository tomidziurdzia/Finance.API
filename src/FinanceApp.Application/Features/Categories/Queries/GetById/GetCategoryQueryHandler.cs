using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Category;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Categories.Queries.GetById;

public class GetCategoryQueryHandler(ICategoryRepository repository, IAuthService authService, UserManager<User> userManager) : IQueryHandler<GetCategoryQuery, CategoryDto>
{
    public async Task<CategoryDto> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(authService.GetSessionUser());
        if (user == null) throw new UnauthorizedAccessException("User not authenticated");
        
        var category = await repository.Get(user.Id, request.CategoryId, cancellationToken);
        if(category is null)
        {
            throw new Exception("Category doesn't exist");
        }

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Type = category.Type.ToString(),
            Description = category.Description,
        };
    }
}