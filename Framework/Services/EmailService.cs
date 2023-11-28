using Amazon.Runtime;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using AutoMapper;
using Core.Validation;
using Framework.DomainModels;
using Framework.Extensions;
using Framework.Repositories;
using Framework.Services.Base;
using Microsoft.Extensions.Options;
using System;

namespace Framework.Services
{
    public class EmailService : INotificationService
    {
        public AuthMessageSenderOptions Options { get; }

        private IAmazonSimpleEmailService _amazonSimpleEmailService;
        private NotificationService _notificationService;
        private readonly IMapper _mapper;
        private readonly ItemRepository _itemRepository;
        private readonly EmailBuilderService _emailBuilderService;

        public EmailService(IOptions<AuthMessageSenderOptions> optionsAccessor,
            NotificationService notificationService,
            IMapper mapper,
            ItemRepository itemRepository,
            EmailBuilderService emailBuilderService)
        {
            Options = optionsAccessor.Value;
            _notificationService = notificationService;
            _mapper = mapper;
            _itemRepository = itemRepository;
            _emailBuilderService = emailBuilderService;
            _amazonSimpleEmailService = new AmazonSimpleEmailServiceClient(new BasicAWSCredentials(Options.Email!.Todoos_Key, Options.Email.Todoos_Sectret_Key), Amazon.RegionEndpoint.EUNorth1);
        }

        public async Task SendAsync(string title, string message, MessageType messageType, int? disyplayTime = null, params string[] recipients)
            => await SendAsync(title, message, messageType, disyplayTime, recipients.Select(r => (r, Guid.Empty)).ToArray());

        public async Task SendAsync(string title, string message, MessageType messageType, int? disyplayTime = null, params (string Email, Guid Id)[] recipients)
        {
            recipients.NotNull();
            recipients!.Satisfies(r => r.Any());
            Options.Email?.Sender_Address.NotNullOrEmpty();

            try
            {
                var response = await _amazonSimpleEmailService.SendEmailAsync(
                    new SendEmailRequest
                    {
                        Destination = new Destination { ToAddresses = recipients.Select(r => r.Email).ToList(), },
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


                var allguids = recipients.Select(r => r.Id).Where(id => id != Guid.Empty).ToArray();
                if (allguids.Any())
                    await _notificationService.SaveNotifications(title, message, allguids);
            }
            catch (Exception ex)
            {
                Console.WriteLine("SendEmailAsync failed with exception: " + ex.Message);
            }
        }

        public async Task SendReminderAsync(Guid itemId, List<(string Email, Guid Id)> recipients)
        {
            var item = _mapper.Map<ToDoItemDomainModel>(await _itemRepository.GetItemCompleteAsync(itemId));
            item.NotNull();

            var nextInfo = item.Schedules
                .Select(s => (Schedule: s, NextOccurrence: s.ScheduleDefinition.NextOccurrenceAfter(DateTime.Now, s.Start, s.End) ?? DateTime.MaxValue))
                .OrderBy(sI => sI.NextOccurrence).First();

            var messageString = _emailBuilderService.BuildErinnerungMailMessage(item, nextInfo.NextOccurrence, nextInfo.Schedule.Type == DomainModels.Common.Enums.ScheduleType.Fixed);
            await SendAsync("Erinnerung", messageString, MessageType.Info, recipients: recipients.ToArray());
        }
    }
}
