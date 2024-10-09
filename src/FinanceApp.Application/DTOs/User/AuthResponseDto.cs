namespace FinanceApp.Application.DTOs.User;

public class AuthResponseDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Lastname { get; set; }
    public string? Email { get; set; }
    public string? Username { get; set; }
    public string? Token { get; set; }
}