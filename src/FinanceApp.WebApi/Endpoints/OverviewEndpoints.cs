using Carter;
using FinanceApp.Application.DTOs.Investment;
using FinanceApp.Application.Features.Overview.Queries.GetTotal;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.WebApi.Endpoints;

public class OverviewEndpoints : ICarterModule
{
    private const string OpenApiTag = "Overview";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var investmentGroup = app.MapGroup("/overviews");
        
        investmentGroup.MapGet("/", async ([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] Guid[]? categoryIds, [FromServices] IMediator mediator) =>
            {
                var query = new GetTotalQuery()
                {
                    StartDate = startDate,
                    EndDate = endDate,
                };

                var result = await mediator.Send(query);
                return Results.Ok(result);
            })
            .WithTags(OpenApiTag)
            .WithName("GetTotals")
            .Produces<List<InvestmentDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequireAuthorization();

    }
}