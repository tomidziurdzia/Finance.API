using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Category;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Categories.Queries.GetAll;

public class GetCategoriesQueryHandler(ICategoriesRepository repository, IAuthService authService, UserManager<User> userManager) : IQueryHandler<GetCategoriesQuery, List<CategoryDto>>
{
    public async Task<List<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(authService.GetSessionUser());
        if (user == null) throw new UnauthorizedAccessException("User not authenticated");

        var categories = await repository.GetAll(user.Id ,cancellationToken);

        return categories.Select(category => new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Type = category.Type.ToString(),
            Description = category.Description,
            UserId = category.UserId,
        }).ToList();
    }
}