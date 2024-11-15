namespace FinanceApp.Application.DTOs.Wallet;

public class WalletsDto
{
    public List<SimpleWalletDto> Wallets { get; set; } = new List<SimpleWalletDto>();
}

public class SimpleWalletDto
{
    public Guid? Id { get; set; } 
    public string Name { get; set; } 
    public string Currency { get; set; }
    public decimal Total { get; set; }
}