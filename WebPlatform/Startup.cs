using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebPlatform.Startup))]
namespace WebPlatform
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
