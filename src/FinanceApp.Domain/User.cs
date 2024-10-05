using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Domain;

public class User : IdentityUser
{
    public string? Name { get; set; }
    public string? Lastname { get; set; }
}