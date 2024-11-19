using FluentValidation;

namespace FinanceApp.Application.Features.InvestmentAccounts.Commands.UpdateInvestmentAccount;

public class UpdateInvestmentAccountCommandValidator : AbstractValidator<UpdateInvestmentAccountCommand>
{
    public UpdateInvestmentAccountCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Investment account name is required")
            .MaximumLength(50).WithMessage("Investment account name must not exceed 50 characters");

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Investment account ID is required");
    }
}