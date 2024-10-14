using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Category;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Categories.Queries.GetAll;

public class GetAllQueryHandler(ICategoriesRepository repository, IAuthService authService, UserManager<User> userManager) : IQueryHandler<GetAllQuery, List<CategoryDto>>
{
    public async Task<List<CategoryDto>> Handle(GetAllQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(authService.GetSessionUser());
        if (user == null) throw new UnauthorizedAccessException("User not authenticated");

        var categories = await repository.GetAll(user.Id ,cancellationToken);

        return categories.Select(category => new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            UserId = category.UserId,
        }).ToList();
    }
}