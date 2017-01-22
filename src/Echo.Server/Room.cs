using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Echo.Server
{
    public class Room
    {
        public Room(string id)
        {
            Id = id;
        }

        public string Id { get; private set; }

        public IEnumerable<string> VisitorsConnectionIds => Visitors.Keys;

        public IDisposable Disposable { get; set; }

        private ConcurrentDictionary<string, string> Visitors { get; } = new ConcurrentDictionary<string, string>();

        public bool AddClient(string connectionId, string clientId)
        {
            return Visitors.TryAdd(connectionId, clientId);
        }
    }
}
