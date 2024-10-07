using Carter;
using FinanceApp.Application.DTOs.User;
using FinanceApp.Application.Features.Users.Queries.GetUsers;
using MediatR;

namespace FinanceApp.WebApi.Endpoints.Users;

public record GetUsersResponse(UserDto[] Users);

public class GetUsersEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/users", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetUsersQuery());

                var response = new GetUsersResponse(result);

                return Results.Ok(response);
            })
            .WithName("GetAllUsers")
            .Produces<GetUsersResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get all users")
            .WithDescription("This endpoint returns all registered users.");

    }
}