using FinanceApp.Application.DTOs.User;
using MediatR;

namespace FinanceApp.Application.Features.Auths.Commands.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, UserDto>
{
    public Task<UserDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}