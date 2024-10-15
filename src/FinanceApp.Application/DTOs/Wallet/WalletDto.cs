using FinanceApp.Domain.Models.Enums;

namespace FinanceApp.Application.DTOs.Wallet;

public class WalletDto
{
    public string Name { get; set; }
    public string UserId { get; set; }
    public Currency Currency { get; set; }
}