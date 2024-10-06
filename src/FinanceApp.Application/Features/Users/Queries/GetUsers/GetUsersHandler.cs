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

            // Mapea las entidades User a UserDto
            var userDtos = users.Select(user => new UserDto(
                Id: new Guid(user.Id),
                Name: user.Name!,
                Lastname: user.Lastname!,
                Email: user.Email!
            )).ToList();

            return new GetUsersResult(
                new PaginatedResult<UserDto>(
                    pageIndex,
                    pageSize,
                    totalCount,
                    userDtos));        
        }
    }
}