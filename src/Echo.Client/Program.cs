using System.Linq;
using System.Threading;

using NDesk.Options;

using static System.Console;

namespace Echo.Client
{
    public static class Program
    {
        public static void Main(string[] args)
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
            var cts = new CancellationTokenSource();
            if (string.IsNullOrEmpty(clientId) ||
                string.IsNullOrEmpty(roomId) ||
                extra.Any())
            {
                DisplayHelp();
            }
            else
            {
                Client.Connect(clientId, roomId, cts.Token);
            }

            WriteLine("Press Enter to exit");
            ReadLine();
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
    }
}
