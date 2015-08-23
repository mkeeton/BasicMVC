using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Authentication.BasicMVC.Startup))]
namespace Authentication.BasicMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
