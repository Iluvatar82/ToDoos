using Framework.Services;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace UI.Web.Areas.Identity.EmailSenderWrapper
{
    public class EmailSenderWrapper : IEmailSender
    {
        private readonly EmailService _emailService;

        public EmailSenderWrapper(EmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task SendEmailAsync(string emails, string subject, string htmlMessage) => await _emailService.SendAsync(subject, htmlMessage, Framework.Services.Base.MessageType.Info, null, emails.Split(";"));
    }
}
