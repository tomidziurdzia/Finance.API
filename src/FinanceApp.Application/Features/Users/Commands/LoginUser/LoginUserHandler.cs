using FinanceApp.Application.Contracts.Identity;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Users.Commands.LoginUser;

public class LoginUserHandler(
    UserManager<User> userManager,
    SignInManager<User> sigInManager,
    RoleManager<IdentityRole> roleManager,
    IAuthService authService)
    : IRequestHandler<LoginUserCommand, LoginUserDto>
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly SignInManager<User> _sigInManager = sigInManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly IAuthService _authService = authService;

    public async Task<LoginUserDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email!);
    }

    private User LoginUser(LoginUserDto loginUserDto)
    {
        var user = await _userManager.FindByEmailAsync(loginUserDto.Email!);
        if (user == null)
        {
            throw new NotFoundEx
        }
    }
}