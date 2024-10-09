using FinanceApp.Domain.Models;

namespace FinanceApp.Application.Contracts.Identity;

public interface IAuthService
{
    string GetSessionUser();

    string CreateToken(User user, IList<string>? roles);
}