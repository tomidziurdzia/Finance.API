using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Transaction;
using FinanceApp.Application.Exceptions;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Transactions.Commands.UpdateTransaction;

public class UpdateTransactionCommandHandler(
    ITransactionRepository transactionRepository,
    IWalletRepository walletRepository,
    ICategoryRepository categoryRepository,
    UserManager<User> userManager,
    IAuthService authService)
    : ICommandHandler<UpdateTransactionCommand, TransactionDto>
{
    public async Task<TransactionDto> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(authService.GetSessionUser());
        if (user == null) throw new UnauthorizedAccessException("User not authenticated");

        var transaction = await transactionRepository.Get(user.Id, request.TransactionId, cancellationToken);
        if (transaction == null || transaction.UserId != user.Id)
        {
            throw new NotFoundException(nameof(Transaction), request.TransactionId);
        }

        var wallet = await walletRepository.Get(user.Id, request.WalletId, cancellationToken);
        if (wallet == null || wallet.UserId != user.Id)
        {
            throw new NotFoundException(nameof(Wallet), request.WalletId);
        }

        var category = await categoryRepository.Get(user.Id, request.CategoryId, cancellationToken);
        if (category == null)
        {
            throw new NotFoundException(nameof(Category), request.CategoryId);
        }

        transaction.WalletId = wallet.Id;
        transaction.CategoryId = category.Id;
        transaction.Amount = request.Amount;
        transaction.Description = request.Description;
        transaction.Type = request.Type;

        await transactionRepository.Update(transaction, cancellationToken);

        return new TransactionDto
        {
            Id = transaction.Id,
            WalletId = transaction.WalletId,
            WalletName = wallet.Name,
            CategoryId = transaction.CategoryId,
            CategoryName = category.Name!,
            UserId = transaction.UserId,
            Type = transaction.Type.ToString(),
            Amount = transaction.Amount,
            Description = transaction.Description,
            CreatedAt = transaction.CreatedAt,
        };
    }
}