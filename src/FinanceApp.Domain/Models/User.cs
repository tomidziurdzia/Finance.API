using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Domain.Models;

public class User : IdentityUser
{
    public string? Name { get; set; }
    public string? Lastname { get; set; }
}