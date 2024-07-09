using Contracts.Configurations;
using Contracts.Services;
using Infrastructure.Configurations;
using MailKit.Net.Smtp;
using MimeKit;
using Serilog;
using Shared.Services.Email;

namespace Infrastructure.Services;

public class SmtpEmailService : ISmtpEmailService
{
    private readonly ILogger _logger;
    private readonly EmailSettings _emailSettings;
    private readonly SmtpClient _smtpClient;

    public SmtpEmailService(ILogger logger, EmailSettings emailSettings)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _emailSettings = emailSettings ?? throw new ArgumentNullException(nameof(emailSettings));
        _smtpClient = new SmtpClient();
    }

    public async Task SendEmailAsync(MailRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
        var emailMessage = new MimeMessage
        {
            Sender = new MailboxAddress(_emailSettings.DisplayName, request.From ?? _emailSettings.From),
            Subject = request.Subject,
            Body = new BodyBuilder
            {
                HtmlBody = request.Body
            }.ToMessageBody()
        };
        if (request.ToAddresses.Any())
        {
            foreach (var toAddress in request.ToAddresses)
            {
                emailMessage.To.Add(MailboxAddress.Parse(toAddress));
            }
        }
        else
        {
            var toAddress = MailboxAddress.Parse(request.ToAddress);
            emailMessage.To.Add(toAddress);
        }

        try
        {
            await _smtpClient.ConnectAsync(_emailSettings.SMTPServer, _emailSettings.Port, _emailSettings.UseSsl,
                cancellationToken);
            await _smtpClient.AuthenticateAsync(_emailSettings.UserName, _emailSettings.Password, cancellationToken);
            await _smtpClient.SendAsync(emailMessage, cancellationToken);
            await _smtpClient.DisconnectAsync(true, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.Error(e.Message, e);
        }
        finally
        {
            await _smtpClient.DisconnectAsync(true, cancellationToken);
            _smtpClient.Dispose();
        }
    }
}