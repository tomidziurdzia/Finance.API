using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Transaction;

namespace FinanceApp.Application.Features.Transactions.Queries.GetAll;

public class GetTransactionsQuery : IQuery<List<TransactionDto>>, IQuery<TransactionDto>
{
    
}