using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Transaction;
using FinanceApp.Application.Exceptions;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Models.Enums;
using FinanceApp.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Transactions.Commands.CreateTransaction;

public class CreateTransactionCommandHandler(
    ITransactionRepository transactionRepository,
    IWalletRepository walletRepository,
    ICategoryRepository categoryRepository,
    UserManager<User> userManager,
    IAuthService authService)
    : ICommandHandler<CreateTransactionCommand, TransactionDto>
{
    public async Task<TransactionDto> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(authService.GetSessionUser());
        if (user == null) throw new UnauthorizedAccessException("User not authenticated");

        var wallet = await walletRepository.Get(user.Id, request.WalletId, cancellationToken);
        if (wallet == null || wallet.UserId != user.Id)
        {
            throw new NotFoundException(nameof(wallet), request.WalletId);
        }

        Category? category = null;
        if (request.CategoryId.HasValue)
        {
            category = await categoryRepository.Get(user.Id, request.CategoryId.Value, cancellationToken);
            if (category == null)
            {
                throw new NotFoundException(nameof(category), request.CategoryId.Value);
            }
        }

        var transaction = new Transaction
        {
            WalletId = wallet.Id,
            CategoryId = category?.Id,
            UserId = user.Id,
            Type = request.Type,
            Amount = request.Type == TransactionType.Income ? request.Amount : -request.Amount,
            Description = request.Description
        };
        
        await transactionRepository.Create(transaction, cancellationToken);

        return new TransactionDto
        {
            Id = transaction.Id,
            WalletId = transaction.WalletId,
            WalletName = wallet.Name,
            CategoryId = transaction?.CategoryId,
            CategoryName = category?.Name,
            UserId = transaction!.UserId,
            Type = transaction.Type.ToString(),
            Amount = transaction.Amount,
            Description = transaction.Description,
            CreatedAt = transaction.CreatedAt
        };
    }
}