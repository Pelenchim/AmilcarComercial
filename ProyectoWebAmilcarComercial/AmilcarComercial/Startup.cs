using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AmilcarComercial.Startup))]
namespace AmilcarComercial
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
