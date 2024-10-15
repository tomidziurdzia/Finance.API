using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Category;
using MediatR;

namespace FinanceApp.Application.Features.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommand : ICommand<Unit>
{
    public Guid Id { get; set; } 
}