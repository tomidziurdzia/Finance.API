using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.User;

namespace FinanceApp.Application.Features.Auths.Users.Queries.GetUserById;

public class GetUserByIdQuery(string userId) : IQuery<AuthResponseDto>
{
    public string? UserId { get; set; } = userId ?? throw new ArgumentNullException(nameof(userId));
}