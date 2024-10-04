using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Domain;

public class User : IdentityUser
{
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
}