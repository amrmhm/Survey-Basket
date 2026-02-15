using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using SurveyBasket.Api.Setting;

namespace SurveyBasket.Api.Health;

public class MailProviderHealthChecks(IOptions<MailSetting> mailSetting) : IHealthCheck
{
    private readonly MailSetting _mailSetting = mailSetting.Value;
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {

        try
        {
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSetting.Host, _mailSetting.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSetting.Mail, _mailSetting.Password);

            return await Task.FromResult(HealthCheckResult.Healthy());

        }
        catch (Exception exception)
        {

            return await Task.FromResult(HealthCheckResult.Unhealthy(exception: exception));


        }
    }
}
