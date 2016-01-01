using Microsoft.Owin;
using Owin;

namespace WhenWillDaddyBeHome
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}