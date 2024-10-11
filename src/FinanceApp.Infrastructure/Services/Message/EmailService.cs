using System.Net;
using System.Net.Mail;
using FinanceApp.Application.Contracts.Infrastructure;
using FinanceApp.Application.Models.Email;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FinanceApp.Infrastructure.Services.Message;

public class EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
    : IEmailService
{
    public EmailSettings _emailSettings { get; } = emailSettings.Value;

    public ILogger<EmailService> _logger { get; } = logger;

    public async Task<bool> SendEmail(EmailMessage emailMessage, string token)
    {
        try
        {
            var fromEmail = _emailSettings.Username;
            var password = _emailSettings.Password;

            var message = new MailMessage();
            message.From = new MailAddress(fromEmail!);
            message.Subject = emailMessage.Subject;
            message.To.Add(new MailAddress(emailMessage.To!));
        
            message.Body = $"{emailMessage.Body} {_emailSettings.BaseUrlClient}/password/reset/{token}";
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = _emailSettings.Port,
                Credentials = new NetworkCredential(fromEmail, password),
                EnableSsl = true,
            };
            await smtpClient.SendMailAsync(message);

            return true;

        }
        catch (Exception)
        {
            _logger.LogError("The email could not be sent");
            return false;
        }
    }
}