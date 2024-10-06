namespace FinanceApp.Application.Dtos.User;

public record LoginUserDto(
    string Email,
    string Password
);