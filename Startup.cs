using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ShopThoiTrang.Startup))]
namespace ShopThoiTrang
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
