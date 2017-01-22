using Echo.Server.Properties;
using Microsoft.Owin.Hosting;

using static System.Console;

namespace Echo.Server
{
    public static class Server
    {
        public static void Run()
        {
            using (WebApp.Start<Startup>(Settings.Default.ServerUri))
            {
                WriteLine("Server running on {0}", Settings.Default.ServerUri);
                WriteLine("Press any key to exit");
                ReadKey(true);
            }
        }
    }
}
