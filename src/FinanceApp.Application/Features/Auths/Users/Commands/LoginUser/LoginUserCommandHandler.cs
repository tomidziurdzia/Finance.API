using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.User;
using FinanceApp.Application.Exceptions;
using FinanceApp.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Auths.Users.Commands.LoginUser;

public class LoginUserCommandHandler(
    UserManager<User> userManager,
    SignInManager<User> sigInManager,
    IAuthService authService)
    : ICommandHandler<LoginUserCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email!);
        if(user is null)
        {
            throw new NotFoundException(nameof(User), request.Email!);
        }

        var result = await sigInManager.CheckPasswordSignInAsync(user, request.Password!, false);

        if(!result.Succeeded)
        {
            throw new Exception("Username not found and/or password incorrect");
        }

        var loginResponse = new AuthResponseDto()
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

        return loginResponse;
    }
}