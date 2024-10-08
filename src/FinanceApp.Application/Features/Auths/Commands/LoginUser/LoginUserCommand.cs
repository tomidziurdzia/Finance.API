using System.Text.Json.Serialization;
using FinanceApp.Application.DTOs.User;
using MediatR;

public class LoginUserCommand(string email, string password) : IRequest<UserDto>
{
    public string Email { get; } = email;
    public string Password { get; } = password;
}