namespace Contracts.Configurations;

public interface IEmailSettings
{
    string DisplayName { get; set; }
    bool EnableVerification { get; set; }
    string From { get; set; }
    string SMTPServer { get; set; }
    bool UseSsl { get; set; }
    int Port { get; set; }
    string UserName { get; set; }
    string Password { get; set; }
}