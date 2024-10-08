using FinanceApp.Application.DTOs.User;
using MediatR;

namespace FinanceApp.Application.Features.Auths.Commands.LoginUser;

public class LoginUserCommand : IRequest<UserDto>
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}