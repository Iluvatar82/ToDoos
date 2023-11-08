using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Framework.Services.Tests
{
    [TestClass()]
    public class ToastNotificationServiceTests
    {
        [TestMethod()]
        public void SendNotificationTest_NoToast_Ok()
        {
            var service = new ToastNotificationService();

            Assert.IsTrue(!service.DisplayedToasts.Any());
        }

        [TestMethod()]
        public void SendNotificationTest_OneToast_Ok()
        {
            var service = new ToastNotificationService();
            service.SendAsync("Test", "Test-Message", Base.MessageType.Warning);

            Assert.IsTrue(service.DisplayedToasts.Any());
        }

        [TestMethod()]
        public void SendNotificationTest_ThreeToast_Ok()
        {
            var service = new ToastNotificationService();
            service.SendAsync("Test", "Test-Message #1", Base.MessageType.Info, 0);
            service.SendAsync("Test", "Test-Message #2", Base.MessageType.Warning, 0);
            service.SendAsync("Test", "Test-Message #3", Base.MessageType.Error, 0);

            Assert.IsTrue(service.DisplayedToasts.Any());
            Assert.IsTrue(service.DisplayedToasts.Count == 3);
        }

        [TestMethod()]
        public async Task SendNotificationTest_ThreeToast_Then_Removed_Ok()
        {
            var service = new ToastNotificationService();
            service.SendAsync("Test", "Test-Message #1", Base.MessageType.Info, 10);
            service.SendAsync("Test", "Test-Message #2", Base.MessageType.Warning, 10);
            service.SendAsync("Test", "Test-Message #3", Base.MessageType.Error, 10);

            Assert.IsTrue(service.DisplayedToasts.Any());
            Assert.IsTrue(service.DisplayedToasts.Count == 3);

            await Task.Delay(100);

            Assert.IsTrue(!service.DisplayedToasts.Any());
        }
    }
}