using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

using Echo.Client.Properties;
using Microsoft.AspNet.SignalR.Client;

using static System.Console;

namespace Echo.Client
{
    public static class Client
    {
        public static async void Connect(string clientId, string roomId, CancellationToken token)
        {
            // ReSharper disable AccessToDisposedClosure
            using (var connection = new Connection(Settings.Default.ServiceUri + "/echo", $"clientId={clientId}&roomId={roomId}"))
            using (Observable.FromEvent<string>(h => connection.Received += h, h => connection.Received -= h)
                             .Subscribe(msg => WriteLine($"Message received from room #{roomId}: {msg}")))
            {
                WriteLine("Connecting...");
                await connection.Start().ContinueWith(_ => WriteLine("Connected"), TaskContinuationOptions.NotOnFaulted).ConfigureAwait(false);
                var message = $"message from clientId={clientId}";
                var everyNMilliseconds = Observable.Interval(Settings.Default.SendInterval);
                var sendDisposable = everyNMilliseconds.Subscribe(async _ => await connection.Send(message));
                await Observable.While(() => !token.IsCancellationRequested, everyNMilliseconds);

                sendDisposable.Dispose();
            }
        }
    }
}
