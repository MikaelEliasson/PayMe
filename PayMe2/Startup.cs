using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PayMe2.Startup))]
namespace PayMe2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
