using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Category;
using FinanceApp.Application.Exceptions;
using FinanceApp.Domain.Repositories;
using FinanceApp.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandHandler(
    ICategoryRepository categoryRepository,
    UserManager<User> userManager,
    IAuthService authService)
    : ICommandHandler<UpdateCategoryCommand, CategoryDto>
{
    public async Task<CategoryDto> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(authService.GetSessionUser());
        if (user == null) throw new UnauthorizedAccessException("User not authenticated");

        var category = await categoryRepository.Get(user.Id, request.Id, cancellationToken);
        if (category == null) throw new NotFoundException(nameof(Category), request.Id);

        category.Name = request.Name;
        category.Description = request.Description ?? category.Description;
        category.Type = request.Type;

        await categoryRepository.Update(category, cancellationToken);

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            Type = category.Type.ToString()
        };
    }
}