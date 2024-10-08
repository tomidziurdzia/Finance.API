using FinanceApp.Domain.Entities;

namespace FinanceApp.Domain.Repositories;

public interface IAuthRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task<bool> ValidatePasswordAsync(User user, string password);
}