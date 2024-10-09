using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.User;

namespace FinanceApp.Application.Features.Auths.Users.Commands.LoginUser;

public class LoginUserCommand : ICommand<UserDto>
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}