namespace FinanceApp.Application.Dtos;

public record UserDto(
    Guid Id,
    string Name,
    string Lastname,
    string Email
    );