using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Transaction;

namespace FinanceApp.Application.Features.Transactions.Queries.GetTransactionById;

public class GetTransactionQuery : IQuery<TransactionDto>
{
    public Guid TransactionId { get; set; }

    public GetTransactionQuery(Guid transactionId)
    {
        TransactionId = transactionId;
    }
}