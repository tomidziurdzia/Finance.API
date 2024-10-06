using FinanceApp.Application.Pagination;

namespace FinanceApp.Application.Features.Users.Queries.GetUsers;

public record GetUsersQuery(PaginationRequest PaginationRequest) : IQuery<GetUsersResult>;

public record GetUsersResult(PaginatedResult<UserDto> Users);
