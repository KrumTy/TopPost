using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TopPost.Web.Startup))]
namespace TopPost.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
