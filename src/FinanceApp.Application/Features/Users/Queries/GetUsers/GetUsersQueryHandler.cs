using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.User;
using FinanceApp.Domain.Services;

namespace FinanceApp.Application.Features.Users.Queries.GetUsers;

public class GetUsersQueryHandler(IUserService userService) : IQueryHandler<GetUsersQuery, UserDto[]>
{
    public async Task<UserDto[]> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await userService.GetAll(); // Llama al servicio, el servicio llama al repositorio

        return users.Select(user => new UserDto
        {
            Name = user.Name!,
            Email = user.Email!
        }).ToArray();
    }
}