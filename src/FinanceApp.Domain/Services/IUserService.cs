﻿using FinanceApp.Domain.Entities;

namespace FinanceApp.Domain.Services;

public interface IUserService
{
    Task<User> Get(string userId);
    Task<User[]> GetAll();
    Task Create(User user, string password);
    Task Update(User user);
    Task UpdatePassword(User user, string newPassword);
    Task Delete(string userId);
}