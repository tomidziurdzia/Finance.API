using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Domain.Entities;

public class User : IdentityUser
{
    [Column(TypeName = "NVARCHAR(20)")]
    public string? Name { get; init; }
    [Column(TypeName = "NVARCHAR(20)")]
    public string? Lastname { get; init; }
}