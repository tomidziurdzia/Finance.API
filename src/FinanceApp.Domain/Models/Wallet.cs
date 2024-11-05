using FinanceApp.Domain.Abstractions;
using FinanceApp.Domain.Models.Enums;

namespace FinanceApp.Domain.Models;

public class Wallet : Entity
{
    public string Name { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public Currency Currency { get; set; }
    public ICollection<Income> Income { get; set; } = new List<Income>();
    public ICollection<Expense> Expense { get; set; } = new List<Expense>();
    public ICollection<Investment> Investment { get; set; } = new List<Investment>();
}