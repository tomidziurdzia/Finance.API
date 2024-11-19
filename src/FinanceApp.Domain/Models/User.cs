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
    public ICollection<Income> Income { get; set; } = new List<Income>();
    public ICollection<Expense> Expense { get; set; } = new List<Expense>();
    public ICollection<Investment> Investment { get; set; } = new List<Investment>();
    public ICollection<InvestmentAccount> InvestmentAccounts { get; set; } = new List<InvestmentAccount>();
}