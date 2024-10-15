using Carter;
using FinanceApp.Application.DTOs.Category;
using FinanceApp.Application.Features.Categories.Commands.CreateCategory;
using FinanceApp.Application.Features.Categories.Commands.UpdateCategory;
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
        
        categoryGroup.MapGet("/{id:guid}", async (Guid id, IMediator mediator) =>
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
        
        categoryGroup.MapPost("/", async (CreateCategoryCommand command, IMediator mediator) =>
            {
                var result = await mediator.Send(command);

                return Results.Created($"/categories/{result.Id}", result);
            })
            .WithTags(OpenApiTag)
            .WithName("CreateCategory")
            .Produces<CategoryDto>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create a new category")
            .WithDescription("This endpoint creates a new category and returns the created category.")
            .RequireAuthorization();
        
        categoryGroup.MapPut("/{id:guid}", async (Guid id, UpdateCategoryCommand command, IMediator mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("Category ID mismatch");

                var result = await mediator.Send(command);

                return Results.Ok(result);
            })
            .WithTags(OpenApiTag)
            .WithName("UpdateCategory")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update category name and description")
            .WithDescription("This endpoint allows updating the name and description of a category.")
            .RequireAuthorization();

    }
}