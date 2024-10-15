using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.Exceptions;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommandHandler(
    ICategoriesRepository categoriesRepository,
    UserManager<User> userManager,
    IAuthService authService)
    : ICommandHandler<DeleteCategoryCommand, Unit>
{
    public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(authService.GetSessionUser());
        if (user == null) throw new UnauthorizedAccessException("User not authenticated");

        var category = await categoriesRepository.Get(user.Id, request.Id, cancellationToken);
        if (category == null) throw new NotFoundException(nameof(Category), request.Id);

        await categoriesRepository.Delete(category, cancellationToken);

        return Unit.Value;
    }
}