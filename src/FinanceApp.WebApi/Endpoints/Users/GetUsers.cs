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
        app.MapGet("/users", async (HttpContext context, IMediator mediator) =>
            {
                Console.WriteLine("RECIBE" + context.Request.Headers["Authorization"].ToString());
                var result = await mediator.Send(new GetUsersQuery());


                var claims = context.User?.Claims;
                if (claims != null)
                {
                    foreach (var claim in claims)
                    {
                        Console.WriteLine($"{claim.Type}: {claim.Value}");
                    }
                }

                var userId = context.User?.FindFirst("userId")?.Value; // Obtener el userId del token JWT
                Console.WriteLine("USER" + context.User?.FindFirst("userId")?.Value);
                if (string.IsNullOrEmpty(userId))
                {
                    return Results.Unauthorized();
                }

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