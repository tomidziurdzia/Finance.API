using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Domain.Models;

public class User : IdentityUser
{
    public string? Name { get; init; }
    public string? Lastname { get; init; }
}