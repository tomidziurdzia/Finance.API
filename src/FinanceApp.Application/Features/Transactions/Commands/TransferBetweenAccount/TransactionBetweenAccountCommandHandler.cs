using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.Exceptions;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Models.Enums;
using FinanceApp.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Transactions.Commands.TransferBetweenAccount;

public class TransactionBetweenAccountCommandHandler(IWalletRepository walletRepository, ITransactionRepository transactionRepository, UserManager<User> userManager,
    IAuthService authService)
    : ICommandHandler<TransactionBetweenAccountCommand>
{
    public async Task<Unit> Handle(TransactionBetweenAccountCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(authService.GetSessionUser());
        if (user == null) throw new UnauthorizedAccessException("User not authenticated");
        
        var sourceWallet = await walletRepository.Get(user.Id, request.SourceWalletId, cancellationToken);
        var targetWallet = await walletRepository.Get(user.Id, request.TargetWalletId, cancellationToken);

        if (sourceWallet == null || targetWallet == null)
            throw new NotFoundException(nameof(sourceWallet), request.SourceWalletId);
        if (targetWallet == null)
            throw new NotFoundException(nameof(targetWallet), request.TargetWalletId);

        var balanceSource = sourceWallet.Transactions
            .Where(t => t.Type == TransactionType.Income || t.Type == TransactionType.Expense)
            .Sum(t => t.Amount);
        
        if (balanceSource < request.Amount)
            throw new InvalidOperationException("Insufficient funds");
        
        var transferOut = new Transaction
        {
            WalletId = sourceWallet.Id,
            Amount = -request.Amount,
            Type = TransactionType.Transfer,
            Description = "Transfer from " + targetWallet.Name,
            UserId = user.Id
        };

        var transferIn = new Transaction
        {
            WalletId = targetWallet.Id,
            Amount = request.Amount,
            Type = TransactionType.Transfer,
            Description = "Transfer to " + sourceWallet.Name,
            UserId = user.Id
        };
        
        await transactionRepository.Create(transferOut, cancellationToken);
        await transactionRepository.Create(transferIn, cancellationToken);

        return Unit.Value;
    }
}