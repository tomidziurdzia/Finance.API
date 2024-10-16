using Carter;
using FinanceApp.Application.Features.Wallets.Commands;
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
    }
}