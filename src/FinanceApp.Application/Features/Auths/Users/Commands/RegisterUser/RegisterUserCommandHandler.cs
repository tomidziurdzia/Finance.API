using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.User;
using FinanceApp.Application.Exceptions;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Auths.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler(UserManager<User> userManager, IAuthService authService, ICategoryRepository categoryRepository)
    : ICommandHandler<RegisterUserCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var userExist = await userManager.FindByEmailAsync(request.Email!) == null ? false : true;
        if (userExist) throw new BadRequestException("User's email already exists");

        var user = new User
        {
            Name = request.Name,
            Lastname = request.Lastname,
            UserName = request.Email!.Split('@')[0],
            Email = request.Email,
        };
        
        var result = await userManager.CreateAsync(user!, request.Password!);

        if (!result.Succeeded) throw new Exception("User could not be registered");
        try
        {
            var defaultCategories = await categoryRepository.GetDefaultCategoriesAsync();

            var userCategories = defaultCategories.Select(category => new Category
            {
                Name = category.Name,
                ParentType = category.ParentType,
                Type = category.Type,
                UserId = user.Id,
            }).ToList();
                
            await categoryRepository.AddCategoriesToUser(userCategories);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to assign default categories to user: {ex.Message}");
        }
            
        return new AuthResponseDto()
        {
            Id = user.Id,
            Name = user.Name,
            Lastname = user.Lastname,
            Email = user.Email,
            Username = user.UserName,
            Currency = user.Currency,
            Locale = user.Locale,
            Token = authService.CreateToken(user),
        };
    }
}