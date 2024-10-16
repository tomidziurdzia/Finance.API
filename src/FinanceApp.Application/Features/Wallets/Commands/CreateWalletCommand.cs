using System.Text.Json.Serialization;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Wallet;
using FinanceApp.Domain.Models.Enums;

namespace FinanceApp.Application.Features.Wallets.Commands;

public class CreateWalletCommand : ICommand<WalletDto>
{
    public string Name { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Currency Currency { get; set; }
}