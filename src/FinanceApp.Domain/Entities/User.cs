using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Domain.Entities;

public class User : IdentityUser
{
    public string? Name { get; init; }
    public string? Lastname { get; init; }
}