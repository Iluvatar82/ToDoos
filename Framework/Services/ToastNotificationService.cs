using Framework.Converter;
using Framework.Services.Base;

namespace Framework.Services
{
    public class ToastNotificationService : INotificationService
    {
        public List<Toast> DisplayedToasts { get; set; }
        public EventHandler? ToastsChanged { get; set; }
        private int ToastDefaultDisplayTime {  get; set; }


        public ToastNotificationService()
        {
            DisplayedToasts = new List<Toast>();
            ToastDefaultDisplayTime = ServiceResources.Toast_DefaultDisplayTime.GetInt();
        }


        public void SendNotification(string title, string message, MessageType messageType, int? disyplayTime = null)
        {
            var newToast = new Toast() { Title = title, Message = message, MessageType = messageType, DisplayTime = disyplayTime ?? ToastDefaultDisplayTime };
            if (disyplayTime != null)
                newToast.DisplayTime = disyplayTime.Value;

            newToast.OnRemove += () =>
            {
                if (DisplayedToasts.Contains(newToast))
                    DisplayedToasts.Remove(newToast);

                ToastsChanged?.Invoke(this, EventArgs.Empty);
            };

            DisplayedToasts.Add(newToast);
            ToastsChanged?.Invoke(this, EventArgs.Empty);

            newToast.StartCountdown();
        }
    }

    public class Toast : IDisposable
    {
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public MessageType MessageType { get; set; }

        public int DisplayTime { get; set; } = 4000;

        private System.Timers.Timer? Countdown { get; set; }

        public Action? OnRemove { get; set; } = null;


        public void StartCountdown()
        {
            if (SetCountdown())
                Countdown!.Start();
        }

        private bool SetCountdown()
        {
            if (DisplayTime <= 0)
                return false;

            Countdown = new System.Timers.Timer(DisplayTime);
            Countdown.Elapsed += (_, _) => OnRemove?.Invoke();
            Countdown.AutoReset = false;
            return true;
        }

        public void Dispose() => Countdown?.Dispose();
    }
}
