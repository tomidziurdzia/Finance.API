using FinanceApp.Application.Exceptions;
using FinanceApp.Domain.Entities;
using FinanceApp.Domain.Repositories;
using FinanceApp.Domain.Services;

namespace FinanceApp.Application.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<User> Get(string userId)
    {
        var user = await userRepository.GetByIdAsync(userId);

        if (user == null)
        {
            throw new NotFoundException("User", userId);
        }

        return user;
    }

    public async Task<User[]> GetAll()
    {
        return await userRepository.GetAllAsync();
    }

    public async Task Create(User user, string password)
    {
        // Aquí podrías incluir lógica de negocio adicional antes de crear el usuario
        await userRepository.CreateAsync(user);
    }

    public async Task Update(User user)
    {
        // Lógica de negocio adicional antes de actualizar
        await userRepository.UpdateAsync(user);
    }

    public async Task UpdatePassword(User user, string newPassword)
    {
        // Ejemplo: lógica de cambio de contraseña
        user.PasswordHash = newPassword; // Simplificación, en realidad deberías usar Identity para esto
        await userRepository.UpdateAsync(user);
    }

    public async Task Delete(string userId)
    {
        await userRepository.DeleteAsync(userId);
    }
}