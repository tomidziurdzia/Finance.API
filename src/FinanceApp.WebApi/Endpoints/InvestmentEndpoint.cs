using Carter;
using FinanceApp.Application.DTOs.Investment;
using FinanceApp.Application.Features.Investments.Commands.CreateInvestment;
using FinanceApp.Application.Features.Investments.Commands.DeleteInvestment;
using FinanceApp.Application.Features.Investments.Commands.UpdateInvestment;
using FinanceApp.Application.Features.Investments.Queries.GetAll;
using FinanceApp.Application.Features.Investments.Queries.Get;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.WebApi.Endpoints;

public class InvestmentEndpoint : ICarterModule
{
    private const string OpenApiTag = "Investment";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var investmentGroup = app.MapGroup("/investments");

        investmentGroup.MapPost("/", async (CreateInvestmentCommand command, IMediator mediator) =>
            {
                var result = await mediator.Send(command);
                return Results.Ok(result);
            })
            .WithTags(OpenApiTag)
            .WithName("CreateInvestment")
            .Produces<InvestmentDto>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequireAuthorization();

        investmentGroup.MapGet("/", async ([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] Guid[]? categoryIds, [FromServices] IMediator mediator) =>
            {
                var query = new GetInvestmentsQuery()
                {
                    StartDate = startDate,
                    EndDate = endDate,
                    CategoryIds = categoryIds?.ToList()
                };

                var result = await mediator.Send(query);
                return Results.Ok(result);
            })
            .WithTags(OpenApiTag)
            .WithName("GetAllInvestments")
            .Produces<List<InvestmentDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequireAuthorization();

        investmentGroup.MapGet("/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetInvestmentQuery(id));
                return Results.Ok(result);
            })
            .WithTags(OpenApiTag)
            .WithName("GetInvestmentById")
            .Produces<InvestmentDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization();

        investmentGroup.MapPut("/{id:guid}", async (Guid id, UpdateInvestmentCommand command, IMediator mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("Investment ID mismatch");

                var updatedInvestment = await mediator.Send(command);

                return Results.Ok(updatedInvestment);
            })
            .WithTags(OpenApiTag)
            .WithName("UpdateInvestment")
            .Produces<InvestmentDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update an investment")
            .WithDescription("This endpoint allows updating the wallet, category, amount, and description of an investment and returns the updated investment.")
            .RequireAuthorization();

        investmentGroup.MapDelete("/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                await mediator.Send(new DeleteInvestmentCommand(id));
                return Results.NoContent();
            })
            .WithTags(OpenApiTag)
            .WithName("DeleteInvestment")
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete an investment")
            .WithDescription("This endpoint deletes an investment by its ID.")
            .RequireAuthorization();
    }
}
