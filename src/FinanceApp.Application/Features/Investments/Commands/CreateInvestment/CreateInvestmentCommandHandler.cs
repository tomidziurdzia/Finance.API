using FinanceApp.Application.Contracts.Identity;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.DTOs.Investment;
using FinanceApp.Application.Exceptions;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Investments.Commands.CreateInvestment;

public class CreateInvestmentCommandHandler(
    IInvestmentRepository investmentRepository,
    IWalletRepository walletRepository,
    ICategoryRepository categoryRepository,
    UserManager<User> userManager,
    IAuthService authService)
    : ICommandHandler<CreateInvestmentCommand, InvestmentDto>
{
    public async Task<InvestmentDto> Handle(CreateInvestmentCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(authService.GetSessionUser());
        if (user == null) throw new UnauthorizedAccessException("User not authenticated");

        var wallet = await walletRepository.Get(user.Id, request.WalletId, cancellationToken);
        if (wallet == null || wallet.UserId != user.Id)
        {
            throw new NotFoundException(nameof(Wallet), request.WalletId);
        }

        Category category = null;
        if (request.CategoryId.HasValue)
        {
            category = await categoryRepository.Get(user.Id, request.CategoryId.Value, cancellationToken);
            if (category == null)
            {
                throw new NotFoundException(nameof(Category), request.CategoryId.Value);
            }
        }

        var investment = new Investment
        {
            WalletId = wallet.Id,
            CategoryId = category!.Id,
            UserId = user.Id,
            Amount = request.Amount,
            Description = request.Description,
            CreatedAt = request.Date
        };
        
        await investmentRepository.Create(investment, cancellationToken);

        return new InvestmentDto
        {
            Id = investment.Id,
            WalletId = investment.WalletId,
            WalletName = wallet.Name,
            CategoryId = investment.CategoryId,
            CategoryName = category.Name,
            UserId = investment.UserId,
            Amount = investment.Amount,
            Description = investment.Description,
            Date = investment.CreatedAt
        };
    }
}
