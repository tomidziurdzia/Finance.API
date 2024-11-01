using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.User;

namespace FinanceApp.Application.Features.Auths.Users.Commands.UpdateUser;

public class UpdateUserCommand : ICommand<AuthResponseDto>
{
    public string? Name { get; set; }
    public string? Lastname { get; set; }
    public string? Currency { get; set; }
    public string? Locale { get; set; }
}