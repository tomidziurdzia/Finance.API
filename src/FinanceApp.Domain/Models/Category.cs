using System.Text.Json.Serialization;
using FinanceApp.Domain.Abstractions;
using FinanceApp.Domain.Models.Enums;

namespace FinanceApp.Domain.Models;

public class Category : Entity
{
    public string Name { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CategoryParent ParentType { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CategoryType Type { get; set; }
    public ICollection<Income> Incomes { get; set; } = new List<Income>();
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    public ICollection<Investment> Investments { get; set; } = new List<Investment>();
}