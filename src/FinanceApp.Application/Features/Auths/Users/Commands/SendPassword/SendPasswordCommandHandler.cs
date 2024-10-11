using System.Text;
using FinanceApp.Application.Contracts.Infrastructure;
using FinanceApp.Application.CQRS;
using FinanceApp.Application.Exceptions;
using FinanceApp.Application.Models.Email;
using FinanceApp.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Application.Features.Auths.Users.Commands.SendPassword;

public class SendPasswordCommandHandler(IEmailService emailService, UserManager<User> userManager)
    : ICommandHandler<SendPasswordCommand, string>
{
    private readonly IEmailService _emailService = emailService;
    private readonly UserManager<User> _userManager = userManager;

    public async Task<string> Handle(SendPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email!);
        if (user == null) throw new BadRequestException("User doesn't exist");

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var plainTextBytes = Encoding.UTF8.GetBytes(token);
        token = Convert.ToBase64String(plainTextBytes);

        var emailMessage = new EmailMessage
        {
            To = request.Email,
            Body = "Reset password, click here:",
            Subject = "Change Password"
        };

        var result = await _emailService.SendEmail(emailMessage, token);
        
        if (!result) throw new Exception("Could not send email");

        return $"The email was sent to the account {request.Email}";
    }
}