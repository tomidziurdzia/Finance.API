using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.User;
using FinanceApp.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Auths.Users.Queries.GetUserToken;

public class GetUserTokenQueryHandler(UserManager<User> userManager, IAuthService authService) : IQueryHandler<GetUserTokenQuery, AuthResponseDto>
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly IAuthService _authService = authService;
    public async Task<AuthResponseDto> Handle(GetUserTokenQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(_authService.GetSessionUser());
        if(user is null)
        {
            throw new Exception("User is not authenticated");
        }

        return new AuthResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Lastname = user.Lastname,
            Email = user.Email,
            Username = user.UserName,
            Currency = user.Currency,
            Locale = user.Locale,
            Token = _authService.CreateToken(user),
        };
    }
}