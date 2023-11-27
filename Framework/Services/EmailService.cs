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

namespace Framework.Services
{
    public class EmailService : INotificationService
    {
        public AuthMessageSenderOptions Options { get; }

        private IAmazonSimpleEmailService _amazonSimpleEmailService;
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
            _amazonSimpleEmailService = new AmazonSimpleEmailServiceClient(new BasicAWSCredentials(Options.Email!.Todoos_Key, Options.Email.Todoos_Sectret_Key), Amazon.RegionEndpoint.EUNorth1);
        }

        public async Task SendAsync(string title, string message, MessageType messageType, int? disyplayTime = null, params string[] recipients)
        {
            recipients.NotNull();
            recipients!.Satisfies(r => r.Any());
            Options.Email?.Sender_Address.NotNullOrEmpty();

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
