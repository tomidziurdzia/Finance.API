using FluentValidation;

namespace FinanceApp.Application.Features.Transactions.Commands.CreateTransaction;

public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        RuleFor(x => x.WalletId)
            .NotEmpty().WithMessage("Wallet ID is required");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Invalid transaction type");
    }
}