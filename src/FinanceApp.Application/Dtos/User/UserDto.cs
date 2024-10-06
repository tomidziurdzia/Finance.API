namespace FinanceApp.Application.Dtos.User;

public record UserDto(
    Guid Id,
    string Name,
    string Lastname,
    string Email
    );