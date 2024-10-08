using FinanceApp.Domain.Entities;

namespace FinanceApp.Application.Contracts;

public interface IAuthService
{
    string GetSessionUser();
    string CreateToken(User user);
}