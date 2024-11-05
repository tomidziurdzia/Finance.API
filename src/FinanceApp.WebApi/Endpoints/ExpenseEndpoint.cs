using Carter;
using FinanceApp.Application.DTOs.Expense;
using FinanceApp.Application.Features.Expenses.Commands.CreateExpense;
using FinanceApp.Application.Features.Expenses.Commands.DeleteExpense;
using FinanceApp.Application.Features.Expenses.Commands.UpdateExpense;
using FinanceApp.Application.Features.Expenses.Queries.GetAll;
using FinanceApp.Application.Features.Expenses.Queries.Get;
using MediatR;

namespace FinanceApp.WebApi.Endpoints;

public class ExpenseEndpoint : ICarterModule
{
    private const string OpenApiTag = "Expense";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var expenseGroup = app.MapGroup("/expenses");

        expenseGroup.MapPost("/", async (CreateExpenseCommand command, IMediator mediator) =>
            {
                var result = await mediator.Send(command);
                return Results.Ok(result);
            })
            .WithTags(OpenApiTag)
            .WithName("CreateExpense")
            .Produces<ExpenseDto>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequireAuthorization();

        expenseGroup.MapGet("/", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetExpensesQuery());
                return Results.Ok(result);
            })
            .WithTags(OpenApiTag)
            .WithName("GetAllExpenses")
            .Produces<List<ExpenseDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequireAuthorization();

        expenseGroup.MapGet("/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetExpenseQuery(id));
                return Results.Ok(result);
            })
            .WithTags(OpenApiTag)
            .WithName("GetExpenseById")
            .Produces<ExpenseDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization();

        expenseGroup.MapPut("/{id:guid}", async (Guid id, UpdateExpenseCommand command, IMediator mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("Expense ID mismatch");

                var updatedExpense = await mediator.Send(command);

                return Results.Ok(updatedExpense);
            })
            .WithTags(OpenApiTag)
            .WithName("UpdateExpense")
            .Produces<ExpenseDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update an expense")
            .WithDescription("This endpoint allows updating the wallet, category, amount, and description of an expense and returns the updated expense.")
            .RequireAuthorization();

        expenseGroup.MapDelete("/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                await mediator.Send(new DeleteExpenseCommand(id));
                return Results.NoContent();
            })
            .WithTags(OpenApiTag)
            .WithName("DeleteExpense")
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete an expense")
            .WithDescription("This endpoint deletes an expense by its ID.")
            .RequireAuthorization();
    }
}
