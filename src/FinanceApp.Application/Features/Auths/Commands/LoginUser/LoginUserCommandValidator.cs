using FluentValidation;

namespace FinanceApp.Application.Features.Auths.Commands.LoginUser;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email cannot be null");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password cannot be null");
    }
}