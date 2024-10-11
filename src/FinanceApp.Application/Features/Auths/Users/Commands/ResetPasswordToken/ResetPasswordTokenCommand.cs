using FinanceApp.Application.CQRS;

namespace FinanceApp.Application.Features.Auths.Users.Commands.ResetPasswordToken;

public class ResetPasswordTokenCommand : ICommand<string>
{
    public string? Email { get; set; }
    public string? Token { get; set; }
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }
}