using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Transaction;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Transactions.Queries.GetAll;

public class GetTransactionsQueryHandler(
    ITransactionRepository transactionRepository,
    UserManager<User> userManager,
    IAuthService authService)
    : IQueryHandler<GetTransactionsQuery, List<TransactionDto>>
{
    public async Task<List<TransactionDto>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(authService.GetSessionUser());
        if (user == null) throw new UnauthorizedAccessException("User not authenticated");

        var transactions = await transactionRepository.GetAll(user.Id, cancellationToken);
        
        return transactions.Select(transaction => new TransactionDto
        {
            Id = transaction.Id,
            WalletId = transaction.WalletId,
            WalletName = transaction.Wallet.Name,
            CategoryId = transaction.CategoryId,
            CategoryName = transaction.Category.Name!,
            UserId = transaction.UserId,
            Type = transaction.Type.ToString(),
            Amount = transaction.Amount,
            Description = transaction.Description,
            CreatedAt = transaction.CreatedAt
        }).ToList();

    }
}