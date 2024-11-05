using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Investment;

namespace FinanceApp.Application.Features.Investments.Queries.GetAll;

public class GetInvestmentsQuery : IQuery<List<InvestmentDto>>
{
}