namespace Framework.Services.Base
{
    public interface INotificationService
    {
        public void SendNotification(string title, string message, MessageType messageType, int? disyplayTime = null);
    }
}
