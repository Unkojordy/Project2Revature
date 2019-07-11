using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Insurance_Site.Startup))]
namespace Insurance_Site
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
