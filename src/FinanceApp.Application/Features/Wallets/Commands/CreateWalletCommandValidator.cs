using FluentValidation;

namespace FinanceApp.Application.Features.Wallets.Commands;

public class CreateWalletCommandValidator : AbstractValidator<CreateWalletCommand>
{
    public CreateWalletCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Wallet name is required")
            .MaximumLength(50).WithMessage("Wallet name must not exceed 50 characters");

        RuleFor(x => x.Currency)
            .IsInEnum().WithMessage("Invalid currency selected");
    }
}