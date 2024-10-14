using FinanceApp.Domain.Abstractions;

namespace FinanceApp.Domain.Models;

public class Category : Entity
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string UserId { get; set; }
    public User? User { get; set; }
}