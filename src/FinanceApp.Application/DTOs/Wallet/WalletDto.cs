using FinanceApp.Domain.Models.Enums;

namespace FinanceApp.Application.DTOs.Wallet;

public class WalletDto
{
    public Guid? Id { get; set; }
    public string Name { get; set; }
    public string Currency { get; set; }
}