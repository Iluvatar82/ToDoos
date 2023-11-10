using Core.Validation;
using Framework.Services.Base;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Framework.Services
{
    public class EmailService : INotificationService
    {
        private string _sender;
        private string _password;


        public EmailService()
        { 
            _sender = ServiceResources.Email_Sender;
            _password = ServiceResources.Email_Password;
        }


        public async Task SendAsync(string title, string message, MessageType messageType, int? disyplayTime = null, params string[] recipients)
        {
            recipients.NotNull();
            recipients!.Satisfies(r => r.Any());

            using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
            {
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(_sender, _password);
                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                var mailBody = new MailMessage
                {
                    From = new MailAddress(_sender, "Awesome ToDo's"),
                    Subject = title,
                    SubjectEncoding = Encoding.UTF8,
                    BodyEncoding = Encoding.UTF8,
                    HeadersEncoding = Encoding.UTF8,
                    IsBodyHtml = true,
                    Body = message,
                    Priority = MailPriority.Normal
                };

                foreach (var recipient in recipients)
                    mailBody.To.Add(new MailAddress(recipient));

                await smtpClient.SendMailAsync(mailBody);
            }

            await Task.CompletedTask;
        }
    }
}
