using AutoMapper;
using Core.Validation;
using Framework.DomainModels;
using Framework.DomainModels.Common;
using Framework.Extensions;
using Framework.Repositories;
using Framework.Services.Base;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using ToDo.Data.ToDoData.Entities;

namespace Framework.Services
{
    public class EmailService : INotificationService
    {
        private readonly IMapper mapper;
        private readonly ItemRepository itemRepository;
        private readonly EmailBuilderService emailBuilderService;

        public EmailService(IOptions<AuthMessageSenderOptions> optionsAccessor,
            IMapper mapper,
            ItemRepository itemRepository,
            EmailBuilderService emailBuilderService)
        {
            Options = optionsAccessor.Value;
            this.mapper = mapper;
            this.itemRepository = itemRepository;
            this.emailBuilderService = emailBuilderService;
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
