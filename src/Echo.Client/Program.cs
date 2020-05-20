using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using NDesk.Options;

using static System.Console;

namespace Echo.Client
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var clientId = string.Empty;
            var roomId = string.Empty;
            var options =
                new OptionSet
                    {
                        { "cid=", cid => clientId = cid },
                        { "rid=", rid => roomId = rid },
                        { "?|h|help", _ => DisplayHelp() },
                        { "*", _ => DisplayHelp() }
                    };
            var extra = options.Parse(args);
            if (string.IsNullOrEmpty(clientId) ||
                string.IsNullOrEmpty(roomId) ||
                extra.Any())
            {
                DisplayHelp();
                KeepRunning();
                return;
            }

            var cts = new CancellationTokenSource();
            cts.Token.Register(() => WriteLine("Exiting app"));

            await Client.Connect(clientId, roomId, cts.Token).ConfigureAwait(false);

            KeepRunning();

            cts.Cancel();
        }

        private static void DisplayHelp()
        {
            WriteLine("Usage: Echo.Client.exe [--cid=clientId] [--rid=roomId] [--?|h|help display help]\n");
            WriteLine("Options:");
            WriteLine("\t--cid [clientId]\t any unique string, better Guid");
            WriteLine("\t--rid [roomId]\t Server room identifier");
            WriteLine("\t--h display help");
        }

        private static void KeepRunning()
        {
            WriteLine("Press Enter to exit");
            ReadLine();
        }
    }
}
