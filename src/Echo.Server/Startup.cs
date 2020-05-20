using JetBrains.Annotations;
using Owin;

namespace Echo.Server
{
    [UsedImplicitly]
    public class Startup
    {
        [UsedImplicitly]
        public void Configuration(IAppBuilder app) => app.MapSignalR<EchoConnection>("/echo");
    }
}
