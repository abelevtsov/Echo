using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

using Echo.Server.Properties;

using static System.Console;

namespace Echo.Server
{
    public class RoomManager
    {
        private ConcurrentDictionary<string, Room> Rooms { get; } = new ConcurrentDictionary<string, Room>();

        public static RoomManager Instance { get; } = new RoomManager();

        private RoomManager()
        {
        }

        public Room GetRoomById(string roomId)
        {
            var room = Rooms.GetOrAdd(roomId, r => new Room(r));
            room.Disposable?.Dispose();
            room.Disposable = RoomRemover(roomId, () => WriteLine("Room #{0} removed", roomId));

            return room;
        }

        public IEnumerable<string> VisitorsExceptRoomVisitors(string roomId)
        {
            var room = GetRoomById(roomId);
            room.Disposable?.Dispose();
            room.Disposable = RoomRemover(roomId, () => WriteLine("Room #{0} removed", roomId));

            return Rooms.SelectMany(r => r.Value.VisitorsConnectionIds).Except(room.VisitorsConnectionIds);
        }

        private IDisposable RoomRemover(string roomId, Action notify)
        {
            Room _;
            return
                Observable.Timer(Settings.Default.Ttl)
                    .Subscribe(
                        n =>
                        {
                            Rooms.TryRemove(roomId, out _);
                            notify?.Invoke();
                        });
        }
    }
}
