using FluentValidation;

namespace FinanceApp.Application.Features.Incomes.Commands.UpdateIncome;

public class UpdateIncomeCommandValidator : AbstractValidator<UpdateIncomeCommand>
{
    public UpdateIncomeCommandValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("The amount must be greater than zero.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("The description is required.")
            .MaximumLength(500).WithMessage("The description must not exceed 500 characters.");

        RuleFor(x => x.WalletId)
            .NotEmpty().WithMessage("A wallet is required.");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("A category is required.");
    }
}