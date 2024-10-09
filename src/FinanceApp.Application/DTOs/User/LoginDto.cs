using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Application.DTOs.User;

public class LoginDto
{
    [Required]
    public string? Email { get; set; }
    [Required]
    public string? Password { get; set; }
}