using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BasicMVC.Startup))]
namespace BasicMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
