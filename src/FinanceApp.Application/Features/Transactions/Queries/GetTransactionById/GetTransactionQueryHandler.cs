using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Transaction;
using FinanceApp.Application.Exceptions;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Transactions.Queries.GetTransactionById;

public class GetTransactionQueryHandler(
    ITransactionRepository transactionRepository,
    UserManager<User> userManager,
    IAuthService authService
    ) : IQueryHandler<GetTransactionQuery, TransactionDto>
{
    public async Task<TransactionDto> Handle(GetTransactionQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(authService.GetSessionUser());
        if (user == null) throw new UnauthorizedAccessException("User not authenticated");
        
        var transaction = await transactionRepository.Get(user.Id, request.TransactionId, cancellationToken);
        if (transaction == null || transaction.UserId != user.Id)
        {
            throw new NotFoundException(nameof(Transaction), request.TransactionId);
        }
        
        return new TransactionDto
        {
            Id = transaction.Id,
            WalletId = transaction.WalletId,
            WalletName = transaction.Wallet.Name,
            CategoryId = transaction.CategoryId,
            CategoryName = transaction.Category?.Name,
            UserId = transaction.UserId,
            Type = transaction.Type.ToString(),
            Amount = transaction.Amount,
            Description = transaction.Description,
            CreatedAt = transaction.CreatedAt
        };
    }
}