using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Category;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Categories.Queries.GetAll;

public class GetCategoriesQueryHandler(ICategoryRepository repository, IIncomeRepository incomeRepository, IInvestmentRepository investmentRepository, IExpenseRepository expenseRepository, IAuthService authService, UserManager<User> userManager) : IQueryHandler<GetCategoriesQuery, List<CategoryDto>>
{
    public async Task<List<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(authService.GetSessionUser());
        if (user == null) throw new UnauthorizedAccessException("User not authenticated");

        var categories = await repository.GetAll(user.Id ,cancellationToken);
        
        var incomes = await incomeRepository.GetAll(
            user.Id,
            request.StartDate,
            request.EndDate,
            request.CategoryIds,
            cancellationToken);

        var investments = await investmentRepository.GetAll(
            user.Id,
            request.StartDate,
            request.EndDate,
            request.CategoryIds,
            cancellationToken);

        var expenses = await expenseRepository.GetAll(
            user.Id,
            request.StartDate,
            request.EndDate,
            request.CategoryIds,
            cancellationToken);

        var categoryDtos = await Task.WhenAll(categories.Select(async category =>
        {
            decimal total = 0;
            if (category.Type.ToString() == "Income")
            {
                total = incomes
                    .Where(income => income.CategoryId == category.Id)
                    .Sum(income => income.Amount);
            }
            else if (category.Type.ToString() == "Investment")
            {
                total = investments
                    .Where(investment => investment.CategoryId == category.Id)
                    .Sum(investment => investment.Amount);
            }
            else if (category.Type.ToString() == "Expense")
            {
                total = expenses
                    .Where(expense => expense.CategoryId == category.Id)
                    .Sum(expense => expense.Amount);
            }

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                ParentType = category.ParentType.ToString(),
                Type = category.Type.ToString(),
                Total = total
            };
        }));

        return categoryDtos
            .OrderByDescending(dto => dto.Total)
            .ThenBy(dto => dto.Name)
            .ToList();
    }
}