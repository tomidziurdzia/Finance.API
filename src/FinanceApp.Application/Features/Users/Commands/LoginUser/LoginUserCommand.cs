using FluentValidation;

namespace FinanceApp.Application.Features.Users.Commands.LoginUser;

public record LoginUserCommand(LoginUserDto LoginUser) : ICommand<LoginUserResult>, IRequest<LoginUserDto>;

public record LoginUserResult();

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.LoginUser.Email).NotEmpty().WithMessage("Email is required");
        RuleFor(x => x.LoginUser.Password).NotEmpty().WithMessage("Password is required");
    }
}