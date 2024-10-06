using FinanceApp.Application.Pagination;
using FinanceApp.Application.Features.Users.Queries.GetUsers;

namespace FinanceApp.WebApi.Endpoints.Users;

public record GetUsersResponse(PaginatedResult<UserDto> Users);

public class GetUsers : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/users", async ([AsParameters] PaginationRequest request, ISender sender) =>
            {
                var result = await sender.Send(new GetUsersQuery(request));

                var response = result.Adapt<GetUsersResponse>();

                return Results.Ok(response);
            })
            .WithName("GetUsers")
            .Produces<GetUsersResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Users")
            .WithDescription("Get Users");
    }
}