using System.Text;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.Exceptions;
using FinanceApp.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Auths.Users.Commands.ResetPasswordToken;

public class ResetPasswordTokenCommandHandler(UserManager<User> userManager)
    : ICommandHandler<ResetPasswordTokenCommand, string>
{
    private readonly UserManager<User> _userManager = userManager;

    public async Task<string> Handle(ResetPasswordTokenCommand request, CancellationToken cancellationToken)
    {
        if (!string.Equals(request.Password, request.ConfirmPassword))
            throw new BadRequestException("Password does not match");

        var updateUser = await _userManager.FindByEmailAsync(request.Email!);
        if (updateUser == null) throw new BadRequestException("The email is not registered");

        var token = Convert.FromBase64String(request.Token!);
        var tokenResult = Encoding.UTF8.GetString(token);

        var result = await _userManager.ResetPasswordAsync(updateUser, tokenResult, request.Password!);
        if (!result.Succeeded) throw new Exception("Password could not be reset");

        return $"Your password has been successfully updated";
    }
}