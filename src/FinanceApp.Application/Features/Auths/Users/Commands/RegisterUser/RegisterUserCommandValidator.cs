using FluentValidation;

namespace FinanceApp.Application.Features.Auths.Users.Commands.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Lastname)
            .NotEmpty().WithMessage("Lastname is required");
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required");
    }
}