using FinanceApp.Domain.Entities;

namespace FinanceApp.Domain.Services;

public interface IAuthService
{
    string CreateToken(User user);
}