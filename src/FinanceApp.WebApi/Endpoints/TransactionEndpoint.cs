using Carter;
using FinanceApp.Application.DTOs.Transaction;
using FinanceApp.Application.Features.Transactions.Commands.CreateTransaction;
using FinanceApp.Application.Features.Transactions.Queries.GetAll;
using FinanceApp.Application.Features.Transactions.Queries.GetTransactionById;
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
        
        transactionGroup.MapGet("/", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetTransactionsQuery());
                return Results.Ok(result);
            })
            .WithTags(OpenApiTag)
            .WithName("GetAllTransactions")
            .Produces<List<TransactionDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequireAuthorization();

        transactionGroup.MapGet("/{transactionId:guid}", async (Guid transactionId, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetTransactionQuery(transactionId));
                return Results.Ok(result);
            })
            .WithTags(OpenApiTag)
            .WithName("GetTransactionById")
            .Produces<TransactionDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization();

    }
}