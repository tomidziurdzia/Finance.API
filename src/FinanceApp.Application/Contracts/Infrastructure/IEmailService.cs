using FinanceApp.Application.Models.Email;

namespace FinanceApp.Application.Contracts.Infrastructure;

public interface IEmailService
{
    Task<bool> SendEmail(EmailMessage emailMessage, string token);
}