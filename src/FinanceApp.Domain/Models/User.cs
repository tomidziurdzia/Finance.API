using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Domain.Models;

public class User : IdentityUser
{
    public string? Name { get; set; }
    public string? Lastname { get; set; }
    public string Currency { get; set; } = "EUR";
    public string Locale { get; set; } = "en-EN";
    public ICollection<Category> Categories { get; set; } = new List<Category>();
    public ICollection<Wallet> Wallets { get; set; } = new List<Wallet>();
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}