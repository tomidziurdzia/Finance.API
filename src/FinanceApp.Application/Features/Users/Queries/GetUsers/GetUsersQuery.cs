using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.User;

namespace FinanceApp.Application.Features.Users.Queries.GetUsers;

public class GetUsersQuery : IQuery<UserDto[]>
{
}