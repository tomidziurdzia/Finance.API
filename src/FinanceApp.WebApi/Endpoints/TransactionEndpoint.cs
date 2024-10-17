using Carter;
using FinanceApp.Application.DTOs.Transaction;
using FinanceApp.Application.Features.Transactions.Commands.CreateTransaction;
using FinanceApp.Application.Features.Transactions.Commands.DeleteTransaction;
using FinanceApp.Application.Features.Transactions.Commands.UpdateTransaction;
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

        transactionGroup.MapPut("/{transactionId:guid}", async (Guid transactionId, UpdateTransactionCommand command, IMediator mediator) =>
            {
                if (transactionId != command.TransactionId) return Results.BadRequest("Transaction ID mismatch");

                var updatedTransaction = await mediator.Send(command);

                return Results.Ok(updatedTransaction);
            })
            .WithTags(OpenApiTag)
            .WithName("UpdateTransaction")
            .Produces<TransactionDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update a transaction")
            .WithDescription("This endpoint allows updating the wallet, category, amount, type, and description of a transaction and returns the updated transaction.")
            .RequireAuthorization();

        transactionGroup.MapDelete("/{transactionId:guid}", async (Guid transactionId, IMediator mediator) =>
            {
                await mediator.Send(new DeleteTransactionCommand(transactionId));

                return Results.NoContent();
            })
            .WithTags(OpenApiTag)
            .WithName("DeleteTransaction")
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete a transaction")
            .WithDescription("This endpoint deletes a transaction by its ID.")
            .RequireAuthorization();

    }
}