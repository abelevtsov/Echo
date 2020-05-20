using System.Linq;
using System.Threading.Tasks;

using JetBrains.Annotations;
using Microsoft.AspNet.SignalR;

using static System.Console;

namespace Echo.Server
{
    [UsedImplicitly]
    public class EchoConnection : PersistentConnection
    {
        protected override Task OnReceived(IRequest request, string connectionId, string data)
        {
            var roomId = request.QueryString["roomId"];
            var excludeConnectionIds = RoomManager.Instance.VisitorsExceptRoomVisitors(roomId).Union(new[] { connectionId });

            return Connection.Broadcast(data, excludeConnectionIds.ToArray());
        }

        protected override Task OnConnected(IRequest request, string connectionId)
        {
            var roomId = request.QueryString["roomId"];
            var clientId = request.QueryString["clientId"];

            var room = RoomManager.Instance.GetRoomById(roomId);
            if (room.AddClient(connectionId, clientId))
            {
                WriteLine($"Сlient {clientId} entered room {room.Id}; (ConnectionId={connectionId})");
            }

            return base.OnConnected(request, connectionId);
        }
    }
}
