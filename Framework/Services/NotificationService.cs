using Framework.Converter;
using Framework.DomainModels.Base;
using Framework.Repositories;

namespace Framework.Services
{
    public class NotificationService
    {
        public Action<List<string>>? Callback { get; set; }

        private readonly ModelMapper _modelMapper;
        private readonly NotificationRepository _notificationRepository;

        public NotificationService(ModelMapper modelMapper, NotificationRepository notificationRepository)
        {
            _modelMapper = modelMapper;
            _notificationRepository = notificationRepository;
        }

        public async Task SaveNotifications(string title, string message, params string[] userIds)
        {
            var userGuidIds = userIds.Where(id => !string.IsNullOrWhiteSpace(id)).Select(id => Guid.Parse(id));
            if (userGuidIds.Any())
                await SaveNotifications(title, message, userGuidIds.ToArray());
        }

        public async Task SaveNotifications(string title, string message, params Guid[] userIds)
        {
            var resultList = new List<NotificationDomainModel>();
            foreach (var userId in userIds)
                resultList.Add(new NotificationDomainModel { Title = title, Message = message, UserId = userId, Category = nameof(Base.MessageType.Info), Sender = "UI" });

            await _notificationRepository.AddAndSaveAsync(_modelMapper.MapToArray(resultList));
            
            Callback?.Invoke(userIds.Select(id => id.ToString()).ToList());
        }
    }
}
