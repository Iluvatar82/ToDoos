namespace Framework.Services
{
    public class EventHandlerService
    {
        private Dictionary<string, List<(string HandlerName, Action<object[]> Action)>> handlers { get; set; }

        public EventHandlerService()
        {
            handlers = new Dictionary<string, List<(string HandlerName, Action<object[]> Action)>>();
        }

        public void AddHandler(string eventName, string handlerName, Action<object[]> action)
        {
            if (!handlers.TryGetValue(eventName, out var eventHandlers))
                handlers.TryAdd(eventName, new List<(string HandlerName, Action<object[]> Action)> { (handlerName, action) });
            else if (!eventHandlers.Any(nh => nh.HandlerName == handlerName))
                eventHandlers.Add((handlerName, action));
            else
            {
                eventHandlers.RemoveAll(nh => nh.HandlerName == handlerName);
                eventHandlers.Add((handlerName, action));
            }
        }

        public void RemoveHandler(string eventName, string handlerName)
        {
            if (handlers.TryGetValue(eventName, out var eventHandlers))
            {
                eventHandlers.RemoveAll(nh => nh.HandlerName == handlerName);
                if (eventHandlers.Count == 0)
                    handlers.Remove(eventName);
            }
        }

        public void ResetHandlers(string eventName)
        {
            handlers.Remove(eventName);
        }

        public void RaiseEvent(string eventName, params object[] args)
        {
            if (handlers.TryGetValue(eventName, out var eventHandlers))
                eventHandlers.ForEach(nh => nh.Action(args));
        }
    }
}
