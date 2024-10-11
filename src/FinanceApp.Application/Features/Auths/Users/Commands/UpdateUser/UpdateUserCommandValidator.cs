using FluentValidation;

namespace FinanceApp.Application.Features.Auths.Users.Commands.UpdateUser;

public class UpdateUserCommandValidator  : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Lastname)
            .NotEmpty().WithMessage("Lastname is required");
    }
}