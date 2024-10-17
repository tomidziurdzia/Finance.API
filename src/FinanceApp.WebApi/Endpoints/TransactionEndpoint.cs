using Carter;
using FinanceApp.Application.DTOs.Transaction;
using FinanceApp.Application.Features.Transactions.Commands.CreateTransaction;
using MediatR;

namespace FinanceApp.WebApi.Endpoints;

public class TransactionEndpoint : ICarterModule
{
    private const string OpenApiTag = "Transaction";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var transactionGroup = app.MapGroup("/transactions");

        transactionGroup.MapPost("/", async (CreateTransactionCommand command, IMediator mediator) =>
            {
                var result = await mediator.Send(command);
                return Results.Ok(result);
            })
            .WithTags(OpenApiTag)
            .WithName("CreateTransaction")
            .Produces<TransactionDto>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequireAuthorization();
    }
}