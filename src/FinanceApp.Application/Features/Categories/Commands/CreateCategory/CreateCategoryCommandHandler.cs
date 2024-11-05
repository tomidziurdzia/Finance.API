using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Category;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandler(
    IAuthService authService,
    UserManager<User> userManager,
    ICategoryRepository repository)
    : ICommandHandler<CreateCategoryCommand, CategoryDto>
{
    public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(authService.GetSessionUser());
        if (user == null) throw new UnauthorizedAccessException("User not authenticated");
        
        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            UserId = user.Id,
            ParentType = request.ParentType,
            Type = request.Type
        };

        await repository.Create(category, cancellationToken);

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            ParentType = category.ParentType.ToString(),
            Type = category.Type.ToString(),
        };
    }
}