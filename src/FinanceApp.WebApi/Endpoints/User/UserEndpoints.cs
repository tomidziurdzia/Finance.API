using Carter;
using FinanceApp.Application.DTOs.User;
using FinanceApp.Application.Features.Auths.Users.Commands.LoginUser;
using FinanceApp.Application.Features.Auths.Users.Commands.RegisterUser;
using FinanceApp.Application.Features.Auths.Users.Commands.ResetPassword;
using FinanceApp.Application.Features.Auths.Users.Commands.ResetPasswordToken;
using FinanceApp.Application.Features.Auths.Users.Commands.SendPassword;
using FinanceApp.Application.Features.Auths.Users.Queries.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.WebApi.Endpoints.User;

public class UserEndpoints : ICarterModule
{
    private const string OpenApiTag = "User";
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var userGroup = app.MapGroup("/user");
        
        userGroup.MapPost("/login", async ([FromBody] LoginUserCommand request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);

                return Results.Ok(result);
            })
            .WithTags(OpenApiTag)
            .WithName("LoginUser")
            .Produces<AuthResponseDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Login user")
            .WithDescription("This endpoint returns a user.");
        
        userGroup.MapPost("/register", async ([FromBody] RegisterUserCommand request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);

                return Results.Ok(result);
            })
            .WithTags(OpenApiTag)
            .WithName("RegisterUser")
            .Produces<AuthResponseDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Register user")
            .WithDescription("This endpoint returns a user.");
        
        userGroup.MapGet("/{id}", async (string id, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetUserByIdQuery(id));

                return Results.Ok(result);
            })
            .WithTags(OpenApiTag)
            .WithName("GetUserById")
            .Produces<AuthResponseDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get user by ID")
            .WithDescription("This endpoint returns a user by their ID.")
            .RequireAuthorization();

        userGroup.MapPost("/update-password", async ([FromBody] ResetPasswordCommand request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);

                return Results.Ok(result);
            })
            .WithTags(OpenApiTag)
            .WithName("UpdatePassword")
            .Produces<AuthResponseDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Password")
            .WithDescription("This endpoint returns a user.")
            .RequireAuthorization();

        userGroup.MapPost("/reset-password", async ([FromBody] ResetPasswordTokenCommand request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);

                return Results.Ok(result);
            })
            .WithTags(OpenApiTag)
            .WithName("ResetPassword")
            .Produces<AuthResponseDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Reset Password")
            .WithDescription("This endpoint returns a user.");

        userGroup.MapPost("/forget-password", async ([FromBody] SendPasswordCommand request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);

                return Results.Ok(result);
            })
            .WithTags(OpenApiTag)
            .WithName("ForgetPassword")
            .Produces<AuthResponseDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Forget Password")
            .WithDescription("This endpoint returns a user.");
    }
}