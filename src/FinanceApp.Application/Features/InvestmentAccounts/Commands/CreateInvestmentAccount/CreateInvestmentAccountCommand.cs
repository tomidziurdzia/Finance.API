using System.Text.Json.Serialization;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.InvestmentAccount;
using FinanceApp.Domain.Models.Enums;

namespace FinanceApp.Application.Features.InvestmentAccounts.Commands.CreateInvestmentAccount;

public class CreateInvestmentAccountCommand : ICommand<InvestmentAccountDto>
{
    public string Name { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Currency Currency { get; set; }
}