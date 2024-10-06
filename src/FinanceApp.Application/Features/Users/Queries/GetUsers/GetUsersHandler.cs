using FinanceApp.Application.Dtos.User;
using FinanceApp.Application.Extensions;
using FinanceApp.Application.Pagination;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Application.Features.Users.Queries.GetUsers
{
    public class GetUsersHandler(IApplicationDbContext dbContext) : IQueryHandler<GetUsersQuery, GetUsersResult>
    {
        public async Task<GetUsersResult> Handle(GetUsersQuery query, CancellationToken cancellationToken)
        {
            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;

            var totalCount = await dbContext.Users.LongCountAsync(cancellationToken);

            var users = await dbContext.Users
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var usersDto = users.ToUserDtoList();

            return new GetUsersResult(
                new PaginatedResult<UserDto>(
                    pageIndex,
                    pageSize,
                    totalCount,
                    users.ToUserDtoList()));        
        }
    }
}