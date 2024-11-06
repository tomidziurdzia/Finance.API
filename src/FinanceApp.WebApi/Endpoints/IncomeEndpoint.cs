using Carter;
using FinanceApp.Application.DTOs.Income;
using FinanceApp.Application.Features.Incomes.Commands.CreateIncome;
using FinanceApp.Application.Features.Incomes.Commands.DeleteIncome;
using FinanceApp.Application.Features.Incomes.Commands.UpdateIncome;
using FinanceApp.Application.Features.Incomes.Queries.GetAll;
using FinanceApp.Application.Features.Incomes.Queries.Get;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.WebApi.Endpoints;

public class IncomeEndpoint : ICarterModule
{
    private const string OpenApiTag = "Income";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var incomeGroup = app.MapGroup("/incomes");

        incomeGroup.MapPost("/", async (CreateIncomeCommand command, IMediator mediator) =>
            {
                var result = await mediator.Send(command);
                return Results.Ok(result);
            })
            .WithTags(OpenApiTag)
            .WithName("CreateIncome")
            .Produces<IncomeDto>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequireAuthorization();

        incomeGroup.MapGet("/", async ([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] Guid[]? categoryIds, [FromServices] IMediator mediator) =>
            {
                var query = new GetIncomesQuery
                {
                    StartDate = startDate,
                    EndDate = endDate,
                    CategoryIds = categoryIds?.ToList()
                };

                var result = await mediator.Send(query);
                return Results.Ok(result);
            })
            .WithTags(OpenApiTag)
            .WithName("GetAllIncomes")
            .Produces<List<IncomeDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequireAuthorization();



        incomeGroup.MapGet("/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetIncomeQuery(id));
                return Results.Ok(result);
            })
            .WithTags(OpenApiTag)
            .WithName("GetIncomeById")
            .Produces<IncomeDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization();

        incomeGroup.MapPut("/{id:guid}", async (Guid id, UpdateIncomeCommand command, IMediator mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("Income ID mismatch");

                var updatedIncome = await mediator.Send(command);

                return Results.Ok(updatedIncome);
            })
            .WithTags(OpenApiTag)
            .WithName("UpdateIncome")
            .Produces<IncomeDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update an income")
            .WithDescription("This endpoint allows updating the wallet, category, amount, and description of an income and returns the updated income.")
            .RequireAuthorization();

        incomeGroup.MapDelete("/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                await mediator.Send(new DeleteIncomeCommand(id));
                return Results.NoContent();
            })
            .WithTags(OpenApiTag)
            .WithName("DeleteIncome")
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete an income")
            .WithDescription("This endpoint deletes an income by its ID.")
            .RequireAuthorization();
    }
}
