using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RepairShopProject12.Startup))]
namespace RepairShopProject12
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
