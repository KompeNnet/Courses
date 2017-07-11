using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(_5th_registration.Startup))]
namespace _5th_registration
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}