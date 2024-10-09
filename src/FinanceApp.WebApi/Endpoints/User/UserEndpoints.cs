using Carter;
using FinanceApp.Application.DTOs.User;
using FinanceApp.Application.Features.Auths.Users.Commands.LoginUser;
using FinanceApp.Application.Features.Auths.Users.Commands.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.WebApi.Endpoints.User;

public class UserEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/login", async ([FromBody] LoginUserCommand request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);

                return Results.Ok(result);
            })
            .WithName("LoginUser")
            .Produces<AuthResponseDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Login user")
            .WithDescription("This endpoint returns a user.");
        
        app.MapPost("/register", async ([FromBody] RegisterUserCommand request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);

                return Results.Ok(result);
            })
            .WithName("RegisterUser")
            .Produces<AuthResponseDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Register user")
            .WithDescription("This endpoint returns a user.");
    }
}