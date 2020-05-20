using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reactive.Disposables;

namespace Echo.Server
{
    public class Room : IDisposable
    {
        public Room(string id) => Id = id;

        public string Id { get; }

        public IEnumerable<string> VisitorsConnectionIds => Visitors.Keys;

        public SerialDisposable LifeRefresh { get; } = new SerialDisposable();

        private ConcurrentDictionary<string, string> Visitors { get; } = new ConcurrentDictionary<string, string>();

        public bool AddClient(string connectionId, string clientId) => Visitors.TryAdd(connectionId, clientId);

        public void Dispose()
        {
            LifeRefresh.Dispose();
        }
    }
}
