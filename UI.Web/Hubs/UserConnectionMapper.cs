using System.Collections.Generic;

namespace UI.Web.Hubs
{
    public class UserConnectionMapper
    {
        private readonly Dictionary<string, List<string>> userConnections = new Dictionary<string, List<string>>();

        public int Count
        {
            get { return userConnections.Count; }
        }

        public void Add(string key, string connectionId)
        {
            lock (userConnections)
            {
                if (!userConnections.TryGetValue(key, out List<string> connections))
                {
                    connections = new List<string>();
                    userConnections.Add(key, connections);
                }

                lock (connections)
                    connections.Add(connectionId);
            }
        }

        public List<string> GetConnections(List<string> keys) => keys.SelectMany(key => GetConnections(key)).ToList();

        public List<string> GetConnections(string key)
        {
            if (userConnections.TryGetValue(key, out List<string> connections))
                return connections;

            return new List<string>();
        }

        public void Remove(string key, string connectionId)
        {
            lock (userConnections)
            {
                if (!userConnections.TryGetValue(key, out List<string> connections))
                    return;

                lock (connections)
                {
                    connections.Remove(connectionId);
                    if (connections.Count == 0)
                        userConnections.Remove(key);
                }
            }
        }
    }
}
