using Carter;
using FinanceApp.Application.Features.Transfers.Commands;
using MediatR;

namespace FinanceApp.WebApi.Endpoints;

public class TransferEndpoint : ICarterModule
{
    private const string OpenApiTag = "Transfer";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var transferGroup = app.MapGroup("/transfers");

        transferGroup.MapPost("/", async (TransferCommand command, IMediator mediator) =>
            {
                await mediator.Send(command);
                return Results.NoContent();
            })
            .WithTags(OpenApiTag)
            .WithName("CreateTransfer")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequireAuthorization();
    }
}