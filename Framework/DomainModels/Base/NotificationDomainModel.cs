namespace Framework.DomainModels.Base
{
    public class NotificationDomainModel : DomainModelBase
    {
        public Guid UserId { get; set; }
        public DateTime Time { get; set; } = DateTime.Now;
        public string Sender { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public DateTime? Read { get; set; }
    }
}
