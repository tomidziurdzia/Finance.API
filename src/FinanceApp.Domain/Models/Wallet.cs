using FinanceApp.Domain.Abstractions;
using FinanceApp.Domain.Models.Enums;

namespace FinanceApp.Domain.Models;

public class Wallet : Entity
{
    public string Name { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public Currency Currency { get; set; }
}