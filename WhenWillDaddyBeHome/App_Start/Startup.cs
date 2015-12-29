using Microsoft.Owin;
using Owin;

namespace WhenWillDaddyBeHome.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}