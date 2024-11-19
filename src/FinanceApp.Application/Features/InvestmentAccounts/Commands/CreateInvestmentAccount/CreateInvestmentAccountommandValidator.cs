using FluentValidation;

namespace FinanceApp.Application.Features.InvestmentAccounts.Commands.CreateInvestmentAccount;

public class CreateInvestmentAccountCommandValidator : AbstractValidator<CreateInvestmentAccountCommand>
{
    public CreateInvestmentAccountCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Investment account name is required.")
            .MaximumLength(50).WithMessage("Investment account name must not exceed 50 characters.");

        RuleFor(x => x.Currency)
            .IsInEnum().WithMessage("Invalid currency selected.");
    }
}