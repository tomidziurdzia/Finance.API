namespace FinanceApp.Infrastructure.Services.Message;

public class EmailSettings
{
    public string? Username { get; set; }
    public string? Password { get; set; }
    public int Port { get; set; }
    public string? BaseUrlClient { get; set; }
}