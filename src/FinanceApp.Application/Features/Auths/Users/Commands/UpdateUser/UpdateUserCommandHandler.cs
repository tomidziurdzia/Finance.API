using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.User;
using FinanceApp.Application.Exceptions;
using FinanceApp.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Auths.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler(UserManager<User> userManager, IAuthService authService)
    : ICommandHandler<UpdateUserCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var updateUser = await userManager.FindByNameAsync(authService.GetSessionUser());
        if(updateUser == null) throw new BadRequestException("User doesn't exist");

        updateUser.Name = request.Name;
        updateUser.Lastname = request.Lastname;

        var result = await userManager.UpdateAsync(updateUser);

        if(!result.Succeeded) throw new Exception("User could not be updated");

        var userById = await userManager.FindByEmailAsync(updateUser.Email!);

        return new AuthResponseDto()
        {
            Id = userById!.Id,
            Name = userById.Name,
            Lastname = userById.Lastname,
            Email = userById.Email,
            Username = userById.UserName,
            Token = authService.CreateToken(userById),
        };
    }
}