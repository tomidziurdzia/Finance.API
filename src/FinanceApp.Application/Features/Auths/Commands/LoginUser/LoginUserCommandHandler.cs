using FinanceApp.Application.Contracts;
using FinanceApp.Application.DTOs.User;
using FinanceApp.Application.Exceptions;
using FinanceApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Auths.Commands.LoginUser;

public class LoginUserCommandHandler(
    UserManager<User> userManager,
    SignInManager<User> sigInManager,
    IAuthService authService)
    : IRequestHandler<LoginUserCommand, UserDto>
{
    public async Task<UserDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email!);
        if(user is null)
        {
            throw new NotFoundException(nameof(User), request.Email!);
        }
        
        var result = await sigInManager.CheckPasswordSignInAsync(user, request.Password!, false);

        if(!result.Succeeded)
        {
            throw new Exception("User credentials are incorrect");
        }

        var userResponse = new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Lastname = user.Lastname,
            Email = user.Email,
            Token = authService.CreateToken(user)
        };

        return userResponse;
    }
}