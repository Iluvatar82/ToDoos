namespace Framework.Services
{
    public class EventHandlerService
    {
        private Dictionary<string, List<Action<object[]>>> handlers { get; set; }

        public EventHandlerService()
        {
            handlers = new Dictionary<string, List<Action<object[]>>>();
        }

        public void AddHandler(string eventName, Action<object[]> action)
        {
            if (!handlers.TryGetValue(eventName, out var eventHandlers))
                handlers.TryAdd(eventName, new List<Action<object[]>> { action });
            else
                eventHandlers.Add(action);
        }

        public void RemoveHandler(string eventName, Action<object[]> action)
        {
            if (handlers.TryGetValue(eventName, out var eventHandlers))
            {
                eventHandlers.Remove(action);
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
                eventHandlers.ForEach(h => h(args));
        }
    }
}
