
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using SurveyBasket.Api.Setting;

namespace SurveyBasket.Api.Services;

public class EmailServices(IOptions<MailSetting> mailSetting) : IEmailSender
{
    private readonly MailSetting _mailSetting = mailSetting.Value;

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var massage = new MimeMessage
        {
            Sender = MailboxAddress.Parse(_mailSetting.Mail),
            Subject = subject
        };
        massage.To.Add(MailboxAddress.Parse(email));

        var builder = new BodyBuilder()
        {
            HtmlBody = htmlMessage
        };

        massage.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();
        smtp.Connect(_mailSetting.Host, _mailSetting.Port, SecureSocketOptions.StartTls);
        smtp.Authenticate(_mailSetting.Mail, _mailSetting.Password );
      

        //Send Massage 
       await smtp.SendAsync(massage);

        //Disconnect Connection
        smtp.Disconnect(true);
    }
}
