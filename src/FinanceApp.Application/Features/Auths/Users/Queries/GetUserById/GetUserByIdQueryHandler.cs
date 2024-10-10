using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.User;
using FinanceApp.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Auths.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler(UserManager<User> userManager) : IQueryHandler<GetUserByIdQuery, AuthResponseDto>
{
    private readonly UserManager<User> _userManager = userManager;

    public async Task<AuthResponseDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId!);
        if(user is null)
        {
            throw new Exception("User doesn't exist");
        }

        return new AuthResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Lastname = user.Lastname,
            Email = user.Email,
            Username = user.UserName,
        };   
    }
}