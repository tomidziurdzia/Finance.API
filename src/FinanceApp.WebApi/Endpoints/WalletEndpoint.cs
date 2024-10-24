using Carter;
using FinanceApp.Application.DTOs.Wallet;
using FinanceApp.Application.Features.Transactions.Commands.TransferBetweenAccount;
using FinanceApp.Application.Features.Wallets.Commands;
using FinanceApp.Application.Features.Wallets.Commands.CreateWallet;
using FinanceApp.Application.Features.Wallets.Commands.DeleteWallet;
using FinanceApp.Application.Features.Wallets.Commands.UpdateWallet;
using FinanceApp.Application.Features.Wallets.Queries.GetWalletById;
using FinanceApp.Application.Features.Wallets.Queries.GetWallets;
using FinanceApp.Application.Features.Wallets.Queries.GetWalletsTotal;
using MediatR;

namespace FinanceApp.WebApi.Endpoints;

public class WalletEndpoint : ICarterModule
{
    private const string OpenApiTag = "Wallet";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var walletGroup = app.MapGroup("/wallets");
        
        walletGroup.MapPost("/", async (CreateWalletCommand command, IMediator mediator) =>
            {
                var result = await mediator.Send(command);

                return Results.Ok(result);

            })
            .WithTags(OpenApiTag)
            .WithName("CreateWallet")
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create a new wallet")
            .WithDescription("This endpoint creates a new wallet for the authenticated user.")
            .RequireAuthorization();
        
        walletGroup.MapGet("/", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetWalletsQuery());

                return Results.Ok(result);
            })
            .WithTags(OpenApiTag)
            .WithName("GetAllWallets")
            .Produces<List<WalletDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequireAuthorization();
        
        walletGroup.MapGet("/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetWalletQuery(id));

                return Results.Ok(result);
            })
            .WithTags(OpenApiTag)
            .WithName("GetWallet")
            .Produces<WalletDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequireAuthorization();
        
        walletGroup.MapPut("/{id:guid}", async (Guid id, UpdateWalletCommand command, IMediator mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("Wallet ID mismatch");

                var result = await mediator.Send(command);

                return Results.Ok(result);
            })
            .WithTags(OpenApiTag)
            .WithName("UpdateWallet")
            .Produces<WalletDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization();

        walletGroup.MapDelete("/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                await mediator.Send(new DeleteWalletCommand(id));
                return Results.NoContent();
            })
            .WithTags(OpenApiTag)
            .WithName("DeleteWallet")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization();

        walletGroup.MapPost("/transfer", async (TransactionBetweenAccountCommand command, IMediator mediator) =>
            {
                await mediator.Send(command);
                return Results.NoContent();
            })
            .WithTags(OpenApiTag)
            .WithName("TransferFunds")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Transfer funds between wallets")
            .WithDescription("Transfers a specified amount from one wallet to another.")
            .RequireAuthorization();

        walletGroup.MapGet("/totals", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetWalletTotalsQuery());
                return Results.Ok(result);
            })
            .WithTags(OpenApiTag)
            .WithName("GetWalletTotals")
            .Produces<List<WalletTotalDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequireAuthorization();

    }
}