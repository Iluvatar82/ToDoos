namespace Framework.Services
{
    public class ThemeService
    {
        private readonly EventHandlerService _eventHandlerService;
        public bool IsDark { get; set; }

        public ThemeService(EventHandlerService eventHandlerService)
        {
            IsDark = false;
            _eventHandlerService = eventHandlerService;
        }

        public void SetIsDark(bool isDark)
        {
            IsDark = isDark;
            _eventHandlerService.RaiseEvent("ThemeChanged", IsDark);
        }
    }
}
