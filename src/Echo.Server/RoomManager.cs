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
            var room = Rooms.GetOrAdd(roomId, rid => new Room(rid));

            RefreshRoomLife(room);

            return room;
        }

        public IEnumerable<string> VisitorsExceptRoomVisitors(string roomId)
        {
            var room = GetRoomById(roomId);

            return Rooms.SelectMany(r => r.Value.VisitorsConnectionIds).Except(room.VisitorsConnectionIds);
        }

        private void RefreshRoomLife(Room room)
        {
            if (room == default)
            {
                return;
            }

            room.LifeRefresh.Disposable = RoomRemover(room.Id, () => WriteLine($"Room #{room.Id} removed"));
        }

        private IDisposable RoomRemover(string roomId, Action notify)
        {
            return
                Observable.Timer(Settings.Default.Ttl)
                    .Subscribe(
                        _ =>
                        {
                            if (!Rooms.TryRemove(roomId, out var room) || room == default)
                            {
                                return;
                            }

                            room.Dispose();
                            notify();
                        });
        }
    }
}
