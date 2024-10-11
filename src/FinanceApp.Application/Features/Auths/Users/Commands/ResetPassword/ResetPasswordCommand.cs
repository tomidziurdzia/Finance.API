using FinanceApp.Application.CQRS;

namespace FinanceApp.Application.Features.Auths.Users.Commands.ResetPassword;

public class ResetPasswordCommand : ICommand
{
    public string? NewPassword { get; set; }
    public string? OldPassword { get; set; }
}