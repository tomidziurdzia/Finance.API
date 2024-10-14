using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Category;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Categories.Queries.GetById;

public class GetByIdQueryHandler(ICategoriesRepository repository, IAuthService authService, UserManager<User> userManager) : IQueryHandler<GetByIdQuery, CategoryDto>
{
    public async Task<CategoryDto> Handle(GetByIdQuery request, CancellationToken cancellationToken)
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
            Description = category.Description,
            UserId = category.UserId
        };
    }
}