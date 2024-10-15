using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Domain.Models;

public class User : IdentityUser
{
    public string? Name { get; set; }
    public string? Lastname { get; set; }
    public ICollection<Category> Categories { get; set; } = new List<Category>();
    public ICollection<Wallet> Wallets { get; set; } = new List<Wallet>();

}