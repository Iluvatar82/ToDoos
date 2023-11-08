namespace Framework.Services.Base
{
    public interface INotificationService
    {
        public Task SendAsync(string title, string message, MessageType messageType, int? disyplayTime = null, params string[] recipients);
    }
}
