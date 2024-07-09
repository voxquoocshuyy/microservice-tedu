namespace Contracts.Services;

public interface IEmailService<in T> where T : class
{
    Task SendEmailAsync(T mailRequest, CancellationToken cancellationToken = new CancellationToken());
}