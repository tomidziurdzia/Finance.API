using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.InvestmentAccount;

namespace FinanceApp.Application.Features.InvestmentAccounts.Queries.GetInvestmentAccounts;

public class GetInvestmentAccountsQuery : IQuery<List<InvestmentAccountDto>>
{
}