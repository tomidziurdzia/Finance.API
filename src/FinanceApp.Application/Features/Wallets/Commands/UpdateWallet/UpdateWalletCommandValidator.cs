using FluentValidation;

namespace FinanceApp.Application.Features.Wallets.Commands.UpdateWallet;

public class UpdateWalletCommandValidator : AbstractValidator<UpdateWalletCommand>
{
    public UpdateWalletCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Wallet name is required")
            .MaximumLength(50).WithMessage("Wallet name must not exceed 50 characters");
        
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Wallet ID is required");
    }
}