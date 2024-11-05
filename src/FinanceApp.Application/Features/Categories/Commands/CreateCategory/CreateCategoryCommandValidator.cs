using FluentValidation;

namespace FinanceApp.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Category name is required")
            .MaximumLength(100).WithMessage("Category name must not exceed 100 characters");

        RuleFor(x => x.Description)
            .MaximumLength(250).WithMessage("Description must not exceed 250 characters");
        
        RuleFor(x => x.ParentType)
            .IsInEnum().WithMessage("Invalid category parent");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Invalid category type");
    }
}
