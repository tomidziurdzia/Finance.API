using Carter;
using FinanceApp.Application.DTOs.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.WebApi.Endpoints.Users;

public class LoginUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/login", async ([FromBody] LoginUserCommand request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);

                return Results.Ok(result);
            })
            .WithName("LoginUser")
            .Produces<UserDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Login user")
            .WithDescription("This endpoint returns a user.");
    }
}