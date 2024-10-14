using Carter;
using FinanceApp.Application.DTOs.Category;
using FinanceApp.Application.Features.Categories.Queries.GetAll;
using FinanceApp.Application.Features.Categories.Queries.GetById;
using MediatR;

namespace FinanceApp.WebApi.Endpoints;

public class CategoryEndpoints : ICarterModule
{
    private const string OpenApiTag = "Category";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var categoryGroup = app.MapGroup("/categories");
        
        categoryGroup.MapGet("/", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetAllQuery());

                return Results.Ok(result);
            })
            .WithTags(OpenApiTag)
            .WithName("GetAllCategories")
            .Produces<CategoryDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get categories")
            .WithDescription("This endpoint returns all categories.")
            .RequireAuthorization();
        
        categoryGroup.MapGet("/{id}", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetByIdQuery(id));

                return Results.Ok(result);
            })
            .WithTags(OpenApiTag)
            .WithName("GetCategoryById")
            .Produces<CategoryDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get category by ID")
            .WithDescription("This endpoint returns a category by their ID.")
            .RequireAuthorization();
    }
}