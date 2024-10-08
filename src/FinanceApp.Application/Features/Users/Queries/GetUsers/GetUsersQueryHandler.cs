using FinanceApp.Application.Contracts;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.User;

namespace FinanceApp.Application.Features.Users.Queries.GetUsers;

public class GetUsersQueryHandler(IUserService userService) : IQueryHandler<GetUsersQuery, UserDto[]>
{
    public async Task<UserDto[]> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await userService.GetAll();

        return users.Select(user => new UserDto
        {
            Name = user.Name!,
            Lastname = user.Lastname!,
            Email = user.Email!
        }).ToArray();
    }
}