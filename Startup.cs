using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DemoBlogTest.Startup))]
namespace DemoBlogTest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
