using System.Net.Mail;

namespace LeaveManagementSystem.Services;

public class EmailSender(IConfiguration _configuration) : IEmailSender
{

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var fromAddress = _configuration["EmailSettings:DefaultEmailAddress"]
            ?? throw new InvalidOperationException("EmailSettings:DefaultEmailAddress is not configured.");

        var smtpServer = _configuration["EmailSettings:Server"]
            ?? throw new InvalidOperationException("EmailSettings:Server is not configured.");

        var smtpPort = int.TryParse(_configuration["EmailSettings:Port"], out var port)
            ? port
            : throw new InvalidOperationException("EmailSettings:Port is not a valid number.");

        var message = new MailMessage
        {
            From = new MailAddress(fromAddress),
            Subject = subject,
            Body = htmlMessage,
            IsBodyHtml = true
        };

        message.To.Add(new MailAddress(email));

        using var client = new SmtpClient(smtpServer, smtpPort);
        await client.SendMailAsync(message);
    }
}
