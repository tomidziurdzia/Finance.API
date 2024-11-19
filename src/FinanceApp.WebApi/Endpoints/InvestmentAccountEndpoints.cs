using Carter;
using FinanceApp.Application.DTOs.InvestmentAccount;
using FinanceApp.Application.Features.InvestmentAccounts.Commands.CreateInvestmentAccount;
using FinanceApp.Application.Features.InvestmentAccounts.Commands.DeleteInvestmentAccount;
using FinanceApp.Application.Features.InvestmentAccounts.Commands.UpdateInvestmentAccount;
using FinanceApp.Application.Features.InvestmentAccounts.Queries.GetInvestmentAccountById;
using FinanceApp.Application.Features.InvestmentAccounts.Queries.GetInvestmentAccounts;
using MediatR;

namespace FinanceApp.WebApi.Endpoints;

public class InvestmentAccountEndpoint : ICarterModule
{
    private const string OpenApiTag = "InvestmentAccount";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var investmentAccountGroup = app.MapGroup("/investment-accounts");
        
        // Create InvestmentAccount
        investmentAccountGroup.MapPost("/", async (CreateInvestmentAccountCommand command, IMediator mediator) =>
            {
                var result = await mediator.Send(command);

                return Results.Created($"/investment-accounts/{result.Id}", result);

            })
            .WithTags(OpenApiTag)
            .WithName("CreateInvestmentAccount")
            .Produces<InvestmentAccountDto>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create a new investment account")
            .WithDescription("This endpoint creates a new investment account for the authenticated user.")
            .RequireAuthorization();
        
        // Get All InvestmentAccounts
        investmentAccountGroup.MapGet("/", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetInvestmentAccountsQuery());

                return Results.Ok(result);
            })
            .WithTags(OpenApiTag)
            .WithName("GetAllInvestmentAccounts")
            .Produces<List<InvestmentAccountDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequireAuthorization();
        
        // Get InvestmentAccount by ID
        investmentAccountGroup.MapGet("/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetInvestmentAccountQuery(id));

                return Results.Ok(result);
            })
            .WithTags(OpenApiTag)
            .WithName("GetInvestmentAccount")
            .Produces<InvestmentAccountDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequireAuthorization();
        
        // Update InvestmentAccount
        investmentAccountGroup.MapPut("/{id:guid}", async (Guid id, UpdateInvestmentAccountCommand command, IMediator mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("InvestmentAccount ID mismatch");

                var result = await mediator.Send(command);

                return Results.Ok(result);
            })
            .WithTags(OpenApiTag)
            .WithName("UpdateInvestmentAccount")
            .Produces<InvestmentAccountDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization();

        // Delete InvestmentAccount
        investmentAccountGroup.MapDelete("/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                await mediator.Send(new DeleteInvestmentAccountCommand(id));
                return Results.NoContent();
            })
            .WithTags(OpenApiTag)
            .WithName("DeleteInvestmentAccount")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization();
    }
}
