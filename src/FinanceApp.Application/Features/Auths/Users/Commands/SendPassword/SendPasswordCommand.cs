using FinanceApp.Application.CQRS;

namespace FinanceApp.Application.Features.Auths.Users.Commands.SendPassword;

public class SendPasswordCommand : ICommand<string>
{
    public string? Email { get; set; }
}