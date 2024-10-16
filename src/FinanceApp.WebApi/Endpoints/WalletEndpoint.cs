using Carter;
using FinanceApp.Application.DTOs.Wallet;
using FinanceApp.Application.Features.Wallets.Commands;
using FinanceApp.Application.Features.Wallets.Queries.GetAll;
using FinanceApp.Application.Features.Wallets.Queries.GetById;
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
    }
}