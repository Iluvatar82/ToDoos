using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using AutoMapper;
using Core.Validation;
using Framework.DomainModels;
using Framework.Extensions;
using Framework.Repositories;
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
        private IAmazonSimpleEmailService _amazonSimpleEmailService;
        private readonly IMapper mapper;
        private readonly ItemRepository itemRepository;
        private readonly EmailBuilderService emailBuilderService;

        public EmailService(IOptions<AuthMessageSenderOptions> optionsAccessor,
            IMapper mapper,
            ItemRepository itemRepository,
            EmailBuilderService emailBuilderService,
            IAmazonSimpleEmailService amazonSimpleEmailService)
        {
            Options = optionsAccessor.Value;
            this.mapper = mapper;
            this.itemRepository = itemRepository;
            this.emailBuilderService = emailBuilderService;
            _amazonSimpleEmailService = amazonSimpleEmailService;
        }

        public AuthMessageSenderOptions Options { get; }


        public async Task SendAsync(string title, string message, MessageType messageType, int? disyplayTime = null, params string[] recipients)
        {
            recipients.NotNull();
            recipients!.Satisfies(r => r.Any());
            Options.Email?.Sender_Address.NotNullOrEmpty();

            //var client = new SendGridClient(Options.SendGridKey);
            //var msg = new SendGridMessage()
            //{
            //    From = new EmailAddress(Options.Email!.Sender_Address, Options.Email!.Sender_Display_Name),
            //    Subject = title,
            //    PlainTextContent = message,
            //    HtmlContent = message
            //};

            //foreach (var toEmail in recipients)
            //    msg.AddTo(new EmailAddress(toEmail));

            //msg.SetClickTracking(false, false);
            //await client.SendEmailAsync(msg);

            //using (var smtpClient = new SmtpClient("email-smtp.eu-north-1.amazonaws.com", 587))
            //{
            //    smtpClient.UseDefaultCredentials = false;
            //    smtpClient.Credentials = new NetworkCredential("todoosadmin", "sOWtC4p86eBqJrY5rdwjIkzu8L3sDbi4vXLcH62r"); //Key: "AKIASQISUTS4PU2MVGPB" //Secret: "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"
            //    smtpClient.EnableSsl = true;
            //    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            //    var mailBody = new MailMessage
            //    {
            //        From = new MailAddress(Options.Email.Sender_Address!, Options.Email.Sender_Display_Name),
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

            try
            {
                var response = await _amazonSimpleEmailService.SendEmailAsync(
                    new SendEmailRequest
                    {
                        Destination = new Destination { ToAddresses = recipients.ToList(), },
                        Message = new Message
                        {
                            Body = new Body
                            {
                                Html = new Amazon.SimpleEmail.Model.Content
                                {
                                    Charset = "UTF-8",
                                    Data = message
                                }
                            },
                            Subject = new Amazon.SimpleEmail.Model.Content
                            {
                                Charset = "UTF-8",
                                Data = title
                            }
                        },
                        Source = $"{Options.Email!.Sender_Display_Name} <{Options.Email.Sender_Address}>",
                    });
            }
            catch (Exception ex)
            {
                Console.WriteLine("SendEmailAsync failed with exception: " + ex.Message);
            }
        }

        public async Task SendReminderAsync(Guid itemId, params string[] recipients)
        {
            var item = mapper.Map<ToDoItemDomainModel>(await itemRepository.GetItemCompleteAsync(itemId));
            item.NotNull();

            var nextInfo = item.Schedules
                .Select(s => (Schedule: s, NextOccurrence: s.ScheduleDefinition.NextOccurrenceAfter(DateTime.Now, s.Start, s.End) ?? DateTime.MaxValue))
                .OrderBy(sI => sI.NextOccurrence).First();

            var messageString = emailBuilderService.BuildErinnerungMailMessage(item, nextInfo.NextOccurrence, nextInfo.Schedule.Type == DomainModels.Common.Enums.ScheduleType.Fixed);
            await SendAsync("Erinnerung", messageString, MessageType.Info, recipients: recipients);
        }
    }
}
