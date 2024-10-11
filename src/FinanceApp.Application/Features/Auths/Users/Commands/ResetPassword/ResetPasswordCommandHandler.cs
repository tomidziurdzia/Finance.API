using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.Exceptions;
using FinanceApp.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Auths.Users.Commands.ResetPassword;

public class ResetPasswordCommandHandler(UserManager<User> userManager, IAuthService authService)
    : ICommandHandler<ResetPasswordCommand>
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly IAuthService _authService = authService;

    public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var updateUser = await _userManager.FindByNameAsync(_authService.GetSessionUser());
        if(updateUser == null)
        {
            throw new BadRequestException("User doesn't exist");
        }
        
        var resultValidateOldPassword = _userManager.PasswordHasher
            .VerifyHashedPassword(updateUser, updateUser.PasswordHash!, request.OldPassword!);

        if(  resultValidateOldPassword != PasswordVerificationResult.Success  )
        {
            throw new BadRequestException("The current password is wrong");
        }

        var hashedNewPassword = _userManager.PasswordHasher.HashPassword(updateUser, request.NewPassword!);
        updateUser.PasswordHash = hashedNewPassword;

        var result = await _userManager.UpdateAsync(updateUser);

        if(!result.Succeeded)
        {
            throw new Exception("Password could not be reset");
        }
        return Unit.Value;
    }
}