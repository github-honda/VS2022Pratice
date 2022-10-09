using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVC48ID.Startup))]
namespace MVC48ID
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
