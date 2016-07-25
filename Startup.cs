using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NgCookingMVC_BackEND.Startup))]
namespace NgCookingMVC_BackEND
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
