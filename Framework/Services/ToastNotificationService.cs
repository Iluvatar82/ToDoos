using Framework.Services.Base;
using System.Timers;

namespace Framework.Services
{
    public class ToastNotificationService : INotificationService, IDisposable
    {
        public Action<string, string, MessageType>? OnShow { get; set; }
        public Action? OnHide { get; set; }
        public System.Timers.Timer? Countdown { get; set; }

        public void SendNotification(string title, string message, MessageType messageType)
        {
            OnShow?.Invoke(title, message, messageType);
            StartCountdown();
        }

        private void StartCountdown()
        {
            SetCountdown();
            if (Countdown!.Enabled)
            {
                Countdown.Stop();
                Countdown.Start();
            }
            else
                Countdown!.Start();
        }

        private void SetCountdown()
        {
            if (Countdown != null)
                return;

            Countdown = new System.Timers.Timer(5000);
            Countdown.Elapsed += HideToast;
            Countdown.AutoReset = false;
        }

        private void HideToast(object? source, ElapsedEventArgs args) => OnHide?.Invoke();

        public void Dispose() => Countdown?.Dispose();
    }
}
