using System.Text.Json.Serialization;
using FinanceApp.Domain.Abstractions;
using FinanceApp.Domain.Models.Enums;

namespace FinanceApp.Domain.Models;

public class Category : Entity
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string UserId { get; set; }
    public User? User { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CategoryType Type { get; set; }
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}