using Core.Validation;
using Framework.Services.Base;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Framework.Services
{
    public class EmailService : INotificationService
    {
        public EmailService(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; }


        public async Task SendAsync(string title, string message, MessageType messageType, int? disyplayTime = null, params string[] recipients)
        {
            recipients.NotNull();
            recipients!.Satisfies(r => r.Any());
            Options.Email?.Sender_Address.NotNullOrEmpty();

            var client = new SendGridClient(Options.SendGridKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(Options.Email!.Sender_Address, Options.Email!.Sender_Display_Name),
                Subject = title,
                PlainTextContent = message,
                HtmlContent = message
            };

            foreach (var toEmail in recipients)
                msg.AddTo(new EmailAddress(toEmail));

            msg.SetClickTracking(false, false);
            await client.SendEmailAsync(msg);

            //using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
            //{
            //    smtpClient.UseDefaultCredentials = false;
            //    smtpClient.Credentials = new NetworkCredential(_sender, _password);
            //    smtpClient.EnableSsl = true;
            //    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            //    var mailBody = new MailMessage
            //    {
            //        From = new MailAddress(_sender, "ToDoos.net"),
            //        Subject = title,
            //        SubjectEncoding = Encoding.UTF8,
            //        BodyEncoding = Encoding.UTF8,
            //        HeadersEncoding = Encoding.UTF8,
            //        IsBodyHtml = true,
            //        Body = message,
            //        Priority = MailPriority.Normal
            //    };

            //    foreach (var recipient in recipients)
            //        mailBody.To.Add(new MailAddress(recipient));

            //    await smtpClient.SendMailAsync(mailBody);
            //}

            //await Task.CompletedTask;

        }
    }
}
