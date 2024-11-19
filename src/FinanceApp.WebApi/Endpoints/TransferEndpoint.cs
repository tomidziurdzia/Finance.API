using Carter;
using FinanceApp.Application.Features.Transfers.Commands.TransferBetweenWallets;
using FinanceApp.Application.Features.Transfers.Commands.TransferFromInvestmentToWallet;
using FinanceApp.Application.Features.Transfers.Commands.TransferWalletToInvestmentAccount;
using MediatR;

namespace FinanceApp.WebApi.Endpoints;

public class TransferEndpoint : ICarterModule
{
    private const string OpenApiTag = "Transfer";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var transferGroup = app.MapGroup("/");

        transferGroup.MapPost("/between-wallets", async (TransferCommand command, IMediator mediator) =>
            {
                await mediator.Send(command);
                return Results.NoContent();
            })
            .WithTags(OpenApiTag)
            .WithName("CreateTransferBetweenWallets")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequireAuthorization();

        transferGroup.MapPost("/to-investment", async (TransferToInvestmentCommand command, IMediator mediator) =>
            {
                await mediator.Send(command);
                return Results.NoContent();
            })
            .WithTags(OpenApiTag)
            .WithName("CreateTransferToInvestmentAccount")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization();

        transferGroup.MapPost("/from-investment", async (TransferFromInvestmentCommand command, IMediator mediator) =>
            {
                await mediator.Send(command);
                return Results.NoContent();
            })
            .WithTags(OpenApiTag)
            .WithName("CreateTransferFromInvestmentAccount")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization();
    }
}
